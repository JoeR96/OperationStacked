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
        public async Task<LinearProgressionExercise> CreateLinearProgressionExercise(
            CreateLinearProgressionExerciseRequest createExerciseModel,
            WorkoutExercise workoutExercise,
        Guid requestUserId = new Guid()
            )
        {
            var _exercise = new LinearProgressionExercise();
            _exercise.WeightIndex = createExerciseModel.WeightIndex;
            _exercise.WorkoutExerciseId = workoutExercise.Id; // or perhaps workoutExercise.Id depending on your structure

            _exerciseRepository.InsertLinearProgressionExercise(_exercise);
            return _exercise;
        }

        public async Task<(LinearProgressionExercise, ExerciseCompletedStatus)> ProgressExercise(CompleteExerciseRequest request)
        {
            var linearProgressionExercise = await _exerciseRepository.GetLinearProgressionExerciseByIdAsync(request.LinearProgressionExerciseId);

            ExerciseCompletedStatus status = ExerciseCompletedStatus.Failed;
            int weightIndexModifier = 0;
            int attemptModifier = 0;
            //if rep target  and set count reached
            //Increase weight index
            if (linearProgressionExercise.SetCountReached(request.Sets) &&
                linearProgressionExercise.TargetRepCountReached(request.Reps) &&
                linearProgressionExercise.WithinRepRange(request.Reps))
            {
                status = ExerciseCompletedStatus.Progressed;
                linearProgressionExercise.CurrentAttempt = 0;
                weightIndexModifier++;
            }
            else if (!linearProgressionExercise.TargetRepCountReached(request.Reps) &&
                     linearProgressionExercise.SetCountReached(request.Sets) &&
                     linearProgressionExercise.WithinRepRange(request.Reps))
            {
                status = ExerciseCompletedStatus.StayedTheSame;
            }
            else if (!linearProgressionExercise.TargetRepCountReached(request.Reps) ||
                !linearProgressionExercise.SetCountReached(request.Sets) || !linearProgressionExercise.WithinRepRange(request.Reps))
            {
                if (linearProgressionExercise.IsLastAttemptBeforeDeload())
                {
                    status = ExerciseCompletedStatus.Deload;
                    weightIndexModifier--;
                    linearProgressionExercise.CurrentAttempt = 0;
                }
                else
                {
                    status = ExerciseCompletedStatus.Failed;
                    attemptModifier++;
                }
            }

            LinearProgressionExercise nextExercise = new LinearProgressionExercise();
            if (linearProgressionExercise.WorkoutExercise.Exercise.EquipmentType is EquipmentType.Cable or EquipmentType.Machine)
            {
                var stack = await _exerciseRepository.GetEquipmentStack(linearProgressionExercise.WorkoutExercise.EquipmentStackId);
                nextExercise = linearProgressionExercise.GenerateNextExercise(await WorkingWeight(linearProgressionExercise.ParentId,linearProgressionExercise.WorkingWeight,linearProgressionExercise.WeightIndex + weightIndexModifier,
                        linearProgressionExercise.WorkoutExercise.WeightProgression
                        , linearProgressionExercise.WorkoutExercise.Exercise.EquipmentType, linearProgressionExercise.WeightIndex,stack),
                    weightIndexModifier, attemptModifier,stack);
            }0
                #',m 0'/*<nbrrrider*/
            else
            {
                nextExercise = linearProgressionExercise.GenerateNextExercise(await WorkingWeight(linearProgressionExercise.ParentId,linearProgressionExercise.WorkingWeight, weightIndexModifier,
                        linearProgressionExercise.WorkoutExercise.WeightProgression
                        , linearProgressionExercise.WorkoutExercise.Exercise.EquipmentType, linearProgressionExercise.WeightIndex),
                    weightIndexModifier, attemptModifier);
            }

            await _exerciseRepository.InsertLinearProgressionExercise(nextExercise);
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
