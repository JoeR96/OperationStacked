using OperationStacked.Entities;

namespace OperationStacked.Extensions.TemplateExtensions
{
    public static class LinearProgressExtensions
    {
        public static bool SetCountReached(this LinearProgressionExercise e, int sets)
            => sets >= e.Sets ? true : false;
        public static bool TargetRepCountReached(this LinearProgressionExercise e, int[] reps)
            => !reps.Any(rep => rep < e.MaximumReps) ? true : false;
        public static bool WithinRepRange(this LinearProgressionExercise e, int[] reps)
            => !reps.Any(rep => rep < e.MinimumReps) ? true : false;

        public static bool IsLastAttemptBeforeDeload(this LinearProgressionExercise e)
            => e.FailedAttempts >= e.AttemptsBeforeDeload;
        public static LinearProgressionExercise GenerateNextExercise(this LinearProgressionExercise e,decimal workingWeight, int weightIndexModifier, int attemptModifier,EquipmentStack stack = null)
            => new()
            {
                MinimumReps = e.MinimumReps,
                MaximumReps = e.MaximumReps,
                Sets = e.Sets,
                WeightIndex = e.WeightIndex + weightIndexModifier,
                PrimaryExercise = e.PrimaryExercise,
                WeightProgression = e.WeightProgression,
                AttemptsBeforeDeload = e.AttemptsBeforeDeload,
                ExerciseName = e.ExerciseName,
                Category = e.Category,
                Template = e.Template,
                LiftDay = e.LiftDay,
                LiftOrder = e.LiftOrder,
                EquipmentStackId = e.EquipmentStackId,
                LiftWeek = e.LiftWeek + 1,
                ParentId = e.Id,
                WorkingWeight = workingWeight,
                FailedAttempts = e.FailedAttempts + attemptModifier,
                EquipmentType = e.EquipmentType,
                UserId = e.UserId
            };
        public static decimal ProgressedWeight(this LinearProgressionExercise exercise)
        {
            return exercise.WorkingWeight + exercise.WeightProgression;
        }
        
        public static decimal DeloadWeight(this LinearProgressionExercise exercise)
        {
            return exercise.WorkingWeight - exercise.WeightProgression;
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
