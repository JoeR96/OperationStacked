using OperationStacked.Entities;

namespace OperationStacked.Extensions.TemplateExtensions
{
    public static class LinearProgressExtensions
    {

        public static bool SetCountReached(this LinearProgressionExercise e, int sets)
            => sets >= e.TargetSets ? true : false;
        public static bool TargetRepCountReached(this LinearProgressionExercise e, int[] reps)
            => !reps.Any(rep => rep < e.MaximumReps) ? true : false;
        public static bool WithinRepRange(this LinearProgressionExercise e, int[] reps)
            => !reps.Any(rep => rep < e.MinimumReps) ? true : false;

        public static bool IsLastAttemptBeforeDeload(this LinearProgressionExercise e)
            => e.CurrentAttempt >= e.AttemptsBeforeDeload;
        public static LinearProgressionExercise GenerateNextExercise(this LinearProgressionExercise e,decimal workingWeight, int weightIndexModifier, int attemptModifier,EquipmentStack stack = null)
            => new LinearProgressionExercise
            {
                MinimumReps = e.MinimumReps,
                MaximumReps = e.MaximumReps,
                TargetSets = e.TargetSets,
                StartingSets = e.TargetSets,
                CurrentSets = e.CurrentSets,
                WeightIndex = e.WeightIndex + weightIndexModifier,
                PrimaryExercise = e.PrimaryExercise,
                StartingWeight = e.StartingWeight,
                WeightProgression = e.WeightProgression,
                AttemptsBeforeDeload = e.AttemptsBeforeDeload,
                ExerciseName = e.ExerciseName,
                Category = e.Category,
                Template = e.Template,
                LiftDay = e.LiftDay,
                LiftOrder = e.LiftOrder,
                EquipmentStackId = stack == null ? Guid.Empty : stack.Id,
                LiftWeek = e.LiftWeek + 1,
                ParentId = e.Id,
                WorkingWeight = workingWeight,
                CurrentAttempt = e.CurrentAttempt + attemptModifier,
                EquipmentType = e.EquipmentType,
                UserId = e.UserId
            };

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
