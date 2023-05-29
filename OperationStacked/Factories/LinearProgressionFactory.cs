using OperationStacked.Abstractions;
using OperationStacked.Entities;
using OperationStacked.Enums;
using OperationStacked.Extensions.TemplateExtensions;
using OperationStacked.Models;
using OperationStacked.Repositories;
using OperationStacked.Requests;

namespace OperationStacked.Factories
{
    public class LinearProgressionFactory : ExerciseFactory<LinearProgressionExercise>
    {
        public LinearProgressionFactory(IExerciseRepository exerciseRepository) : base(exerciseRepository)
        {
        }

        public override bool AppliesTo(Type type) => typeof(LinearProgressionExercise).Equals(type);

        public override async Task<IExercise> CreateExercise(CreateExerciseModel createExerciseModel)
        {
            CreateBaseExercise(createExerciseModel);
            _exercise.MinimumReps = _createExerciseModel.MinimumReps;
            _exercise.MaximumReps = _createExerciseModel.MaximumReps;
            _exercise.TargetSets = _createExerciseModel.TargetSets;
            _exercise.WeightIndex = _createExerciseModel.WeightIndex;
            _exercise.PrimaryExercise = _createExerciseModel.PrimaryExercise;
            _exercise.StartingWeight = _createExerciseModel.StartingWeight;
            _exercise.WeightProgression = _createExerciseModel.WeightProgression;
            _exercise.AttemptsBeforeDeload = _createExerciseModel.AttemptsBeforeDeload;

            if (createExerciseModel.EquipmentType is EquipmentType.Cable or EquipmentType.Machine)
            {
             
                if ((int)_createExerciseModel.EquipmentStackKey is 0)
                {
                    var equipmentStack = await _exerciseRepository.InsertEquipmentStack(createExerciseModel.EquipmentStack);
                    _exercise.EquipmentStackId = equipmentStack.Stack.Id;
                    var stack = await CreateStack(Guid.Empty,0,_exercise.WeightIndex,_exerciseRepository,_exercise.EquipmentStackId);
                    _exercise.StartingWeight = stack;
                }
                else
                {
                    //mapped to cables atm will need to map it to other enum types another day
                    _exercise.EquipmentStackId = Guid.Parse("08db6084-b7f8-4b35-86f1-6c7b1e003d51");
                }
            }

            return _exercise;
        }

        public override async Task<(Exercise, ExerciseCompletedStatus)> ProgressExercise(CompleteExerciseRequest request)
        {
            var exercise = (LinearProgressionExercise)await _exerciseRepository.GetExerciseById(request.Id);
            exercise.Completed = true;
            await _exerciseRepository.UpdateAsync(exercise);
            ExerciseCompletedStatus status = ExerciseCompletedStatus.Active;
            int weightIndexModifier = 0;
            int attemptModifier = 0;
            //if rep target  and set count reached
            //Increase weight index
            if (exercise.SetCountReached(request.Sets) &&
                exercise.TargetRepCountReached(request.Reps) &&
                exercise.WithinRepRange(request.Reps))
            {
                //get next weeks exercise
                //increase weight index
                //save game
                status = ExerciseCompletedStatus.Progressed;
                weightIndexModifier++;
            }
            else if (!exercise.TargetRepCountReached(request.Reps) &&
                exercise.SetCountReached(request.Sets) &&
                exercise.WithinRepRange(request.Reps))
            {
                status = ExerciseCompletedStatus.StayedTheSame;

            }
            else if (!exercise.TargetRepCountReached(request.Reps) ||
                !exercise.SetCountReached(request.Sets))
            {
                if (exercise.IsLastAttemptBeforeDeload())
                {
                    status = ExerciseCompletedStatus.Deload;
                    weightIndexModifier--;
                }
                else
                {
                    status = ExerciseCompletedStatus.Failed;
                    attemptModifier++;
                }
            }
            else
            {
                status = ExerciseCompletedStatus.Failed;
                //Log to the logger here
                //We should not have got here
            }

            var nextExercise = exercise.GenerateNextExercise(await WorkingWeight(exercise.ParentId,exercise.WorkingWeight, weightIndexModifier,
                    exercise.WeightProgression
                    , exercise.EquipmentType, exercise.WeightIndex,
                    _exerciseRepository,
                    exercise.EquipmentStackId),
                weightIndexModifier, attemptModifier);
            await _exerciseRepository.InsertExercise(nextExercise); 
            return (nextExercise, status);
        }
        
        private static async Task<decimal> WorkingWeight(Guid exerciseParentId, decimal workingWeight,
            int weightIndexModifier,
            decimal weightProgression, EquipmentType equipmentType, int startIndex,
            IExerciseRepository exerciseRepository = null,
            Guid equipmentStackId = default)
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
                return workingWeight > 9 ? workingWeight += 2 : workingWeight += 1;
            }

            if (equipmentType is EquipmentType.Cable or EquipmentType.Machine)
            {
                return await CreateStack(exerciseParentId, workingWeight, startIndex, exerciseRepository, equipmentStackId);
            }
            throw new NotImplementedException("EquipmentType is not supported");
        }

        private static async Task<decimal> CreateStack(Guid exerciseParentId, decimal workingWeight, int startIndex,
            IExerciseRepository exerciseRepository, Guid equipmentStackId)
        {
            var stack = await exerciseRepository.GetEquipmentStack(equipmentStackId);
            var generatedStack = stack.GenerateStack();
            int index;

            if (exerciseParentId != Guid.Empty)
            {
                index = Array.IndexOf(generatedStack, workingWeight);
            }
            else
            {
                index = startIndex;
            }

            return (decimal)generatedStack[index];
        }
    }
}
