using OperationStacked.Abstractions;
using OperationStacked.Entities;
using OperationStacked.Enums;
using OperationStacked.Extensions.TemplateExtensions;
using OperationStacked.Models;
using OperationStacked.Repositories;
using OperationStacked.Requests;

namespace OperationStacked.Factories
{
    public class LinearProgressionService : ILinearProgressionService
    {
        private readonly IExerciseRepository _exerciseRepository;
        public LinearProgressionService(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }
        public async Task<LinearProgressionExercise> CreateExercise(CreateExerciseModel createExerciseModel,
            Guid requestUserId = new Guid())
        {
            var _exercise = new LinearProgressionExercise();
            _exercise.PopulateBaseValues(createExerciseModel, requestUserId);
            _exercise.MinimumReps = createExerciseModel.MinimumReps;
            _exercise.MaximumReps = createExerciseModel.MaximumReps;
            _exercise.Sets = createExerciseModel.TargetSets;
            _exercise.WeightIndex = createExerciseModel.WeightIndex;
            _exercise.PrimaryExercise = createExerciseModel.PrimaryExercise;
            _exercise.WeightProgression = createExerciseModel.WeightProgression;
            _exercise.AttemptsBeforeDeload = createExerciseModel.AttemptsBeforeDeload;

            if (createExerciseModel.EquipmentType is EquipmentType.Cable or EquipmentType.Machine)
            {
             
                if ((int)createExerciseModel.EquipmentStackKey is 0)
                {
                    var equipmentStack = await _exerciseRepository.InsertEquipmentStack(createExerciseModel.EquipmentStack);
                    _exercise.EquipmentStackId = equipmentStack.Stack.Id;
                    var stack = CreateStack(Guid.Empty,0,_exercise.WeightIndex,equipmentStack.Stack);
                    _exercise.WorkingWeight = stack;

                }
                else
                {
                    //mapped to cables atm will need to map it to other enum types another day
                     _exercise.EquipmentStackId = Guid.Parse("08db8609-b8c5-481a-8fab-53462d6d37ef");
                      var equipmentStack = await _exerciseRepository.GetEquipmentStack(_exercise.EquipmentStackId);
                      _exercise.EquipmentStackId = _exercise.EquipmentStackId;
                      var stack = CreateStack(Guid.Empty,0,_exercise.WeightIndex,equipmentStack);
                      _exercise.WorkingWeight = stack;

                }
            }

            return _exercise;
        }

        public async Task<(LinearProgressionExercise, ExerciseCompletedStatus)> ProgressExercise(CompleteExerciseRequest request)
        {
            var exercise = (LinearProgressionExercise)await _exerciseRepository.GetExerciseById(request.Id);
            exercise.Completed = true;
            await _exerciseRepository.UpdateAsync(exercise);
            ExerciseCompletedStatus status = ExerciseCompletedStatus.Failed;
            int weightIndexModifier = 0;
            int attemptModifier = 0;
            //if rep target  and set count reached
            //Increase weight index
            if (exercise.SetCountReached(request.Sets) &&
                exercise.TargetRepCountReached(request.Reps) &&
                exercise.WithinRepRange(request.Reps))
            {
                status = ExerciseCompletedStatus.Progressed;
                exercise.CurrentAttempt = 0;
                weightIndexModifier++;
            }
            else if (!exercise.TargetRepCountReached(request.Reps) &&
                exercise.SetCountReached(request.Sets) &&
                exercise.WithinRepRange(request.Reps))
            {
                status = ExerciseCompletedStatus.StayedTheSame;
            }
            else if (!exercise.TargetRepCountReached(request.Reps) ||
                !exercise.SetCountReached(request.Sets) || !exercise.WithinRepRange(request.Reps))
            {
                if (exercise.IsLastAttemptBeforeDeload())
                {
                    status = ExerciseCompletedStatus.Deload;
                    weightIndexModifier--;
                    exercise.CurrentAttempt = 0;
                }
                else
                {
                    status = ExerciseCompletedStatus.Failed;
                    attemptModifier++;
                }
            }

            LinearProgressionExercise nextExercise;
            if (exercise.EquipmentType is EquipmentType.Cable or EquipmentType.Machine)
            {
                var stack = await _exerciseRepository.GetEquipmentStack(exercise.EquipmentStackId);
                nextExercise = exercise.GenerateNextExercise(await WorkingWeight(exercise.ParentId,exercise.WorkingWeight,exercise.WeightIndex + weightIndexModifier,
                        exercise.WeightProgression
                        , exercise.EquipmentType, exercise.WeightIndex,stack),
                    weightIndexModifier, attemptModifier,stack);
                
                
            }
            else
            {
                nextExercise = exercise.GenerateNextExercise(await WorkingWeight(exercise.ParentId,exercise.WorkingWeight, weightIndexModifier,
                        exercise.WeightProgression
                        , exercise.EquipmentType, exercise.WeightIndex),
                    weightIndexModifier, attemptModifier);
            }
            
            await _exerciseRepository.InsertExercise(nextExercise); 
            return (nextExercise, status);
        }
        
        public static async Task<decimal> WorkingWeight(Guid exerciseParentId, decimal workingWeight,
            int weightIndexModifier,
            decimal weightProgression, EquipmentType equipmentType, int weightIndex,
            EquipmentStack stack = null)
        {
            if (equipmentType is EquipmentType.Barbell or EquipmentType.SmithMachine)
            {
                if(weightIndexModifier > 0)
                {
                    return workingWeight += weightProgression;
                }
                else if(weightIndexModifier == 0)
                {
                    return workingWeight;
                }
                else
                {
                    return workingWeight -= weightProgression;
                }
            }

            if (equipmentType == EquipmentType.Dumbbell)
            {
                if(weightIndexModifier > 0)
                {
                    return workingWeight > 9 ? workingWeight += 2 : workingWeight += 1;
                }
                else if(weightIndexModifier == 0)
                {
                    return workingWeight;
                }
                if(weightIndexModifier < 0)
                {
                    return workingWeight > 10 ? workingWeight -= 2 : workingWeight -= 1;
                }
            }

        
            if (equipmentType is EquipmentType.Cable or EquipmentType.Machine)
            {  
                return CreateStaticStack(exerciseParentId, workingWeight, weightIndexModifier, stack);
            }
            throw new NotImplementedException("EquipmentType is not supported");
        }

        public decimal CreateStack(Guid exerciseParentId, decimal workingWeight, int startIndex,
            EquipmentStack stack)
        {
            var generatedStack = stack.GenerateStack();
            int index = startIndex;

            if (index < 0)
            {
                index = 0;
            }

            return (decimal)generatedStack[index];
        }
        
        public static decimal CreateStaticStack(Guid exerciseParentId, decimal workingWeight, int startIndex,
            EquipmentStack stack)
        {
            var generatedStack = stack.GenerateStack();
            int index = startIndex;

            if (index < 0)
            {
                index = 0;
            }

            return (decimal)generatedStack[index];
        }
    }
}
