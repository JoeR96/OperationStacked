using OperationStacked.Entities;
using OperationStacked.Requests;

namespace OperationStacked.Extensions.TemplateExtensions
{
    public static class LinearProgressExtensions
    {
        public static bool SetCountReached(this LinearProgressionExercise e, int sets)
            => sets >= e.WorkoutExercise.Sets ? true : false;
        public static bool TargetRepCountReached(this LinearProgressionExercise e, int[] reps)
            => !reps.Any(rep => rep < e.WorkoutExercise.MaximumReps) ? true : false;
        public static bool WithinRepRange(this LinearProgressionExercise e, int[] reps)
            => !reps.Any(rep => rep < e.WorkoutExercise.MinimumReps) ? true : false;

        public static bool IsLastAttemptBeforeDeload(this LinearProgressionExercise e)
            => e.CurrentAttempt >= e.WorkoutExercise.AttemptsBeforeDeload;

        public static decimal ProgressedWeight(this LinearProgressionExercise exercise)
        {
            return exercise.WorkingWeight + exercise.WorkoutExercise.WeightProgression;
        }
        
        public static decimal DeloadWeight(this LinearProgressionExercise exercise)
        {
            return exercise.WorkingWeight - exercise.WorkoutExercise.WeightProgression;
        }
        public static Decimal?[] GenerateStack(this EquipmentStack e)
        {
            
            List<Decimal?> stack = new List<Decimal?>();
            stack.Add(e.StartWeight);

            foreach (var increment in e.InitialIncrements)
            {
                var t = stack.Last();
                stack.Add(t += increment);
            }

            for (int i = 0; i < e.IncrementCount; i++)
            {
                var t = stack.Last();
                stack.Add(t += e.IncrementValue);
            }

            return stack.ToArray();
        }

        public static LinearProgressionExercise ToLinearProgressionExercise(
            this CreateLinearProgressionExerciseRequest request,
            Guid workoutExerciseId,
            Guid userId)
        {
            var linearProgressionExercise = new LinearProgressionExercise
            {
                WorkoutExerciseId = workoutExerciseId,
                WeightIndex = request.WeightIndex,
                CurrentAttempt = 0,
                ParentId = userId, // or another appropriate value
                LiftWeek = 1, // defaulting to week 1
            };

            if (request.StartingWeight != null)
            {
                linearProgressionExercise.WorkingWeight = (decimal)request.StartingWeight;
            }
            return linearProgressionExercise;
        }

        public static LinearProgressionExercise GenerateNextExercise(this
            LinearProgressionExercise currentExercise,
            decimal workingWeightModifier,
            int weightIndexModifier,
            int attemptModifier)
        {
            return new LinearProgressionExercise
            {
                WorkoutExerciseId = currentExercise.WorkoutExerciseId,
                ParentId = currentExercise.ParentId,
                LiftWeek = currentExercise.LiftWeek + 1, // Assuming LiftWeek should be incremented
                WorkingWeight = currentExercise.WorkingWeight + workingWeightModifier,
                WeightIndex = currentExercise.WeightIndex + weightIndexModifier,
                CurrentAttempt = currentExercise.CurrentAttempt + attemptModifier
            };
        }

        public static LinearProgressionExercise GenerateNextExercise(this
            LinearProgressionExercise currentExercise,
            decimal workingWeightModifier,
            int weightIndexModifier,
            int attemptModifier,
            EquipmentStack equipmentStack)
        {
            var stackWeightModifier = equipmentStack.GenerateStack();
            var weightIndex = currentExercise.WeightIndex + weightIndexModifier;
            return new LinearProgressionExercise
            {
                WorkoutExerciseId = currentExercise.WorkoutExerciseId,
                ParentId = currentExercise.ParentId,
                LiftWeek = currentExercise.LiftWeek + 1, // Assuming LiftWeek should be incremented
                WorkingWeight = (decimal)stackWeightModifier[weightIndex],
                WeightIndex = weightIndex,
                CurrentAttempt = currentExercise.CurrentAttempt + attemptModifier
            };
        }

    }
}
