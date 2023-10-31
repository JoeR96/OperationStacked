using OperationStacked.Entities;

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
        public static LinearProgressionExercise GenerateNextExercise(this LinearProgressionExercise e,decimal workingWeight, int weightIndexModifier, int attemptModifier,EquipmentStack stack = null)
            => new()
            {
                WorkoutExerciseId = e.WorkoutExerciseId,
                CurrentAttempt = e.CurrentAttempt + attemptModifier
            };
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
    }
}
