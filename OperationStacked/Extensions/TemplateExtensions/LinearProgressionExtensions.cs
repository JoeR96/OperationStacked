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
        public static LinearProgressionExercise GenerateNextExercise(this LinearProgressionExercise e, int weightIndexModifier, int attemptModifier)
            => new LinearProgressionExercise
            {
                MinimumReps = e.MinimumReps,
                MaximumReps = e.MaximumReps,
                TargetSets = e.TargetSets,
                StartingSets = e.TargetSets,
                CurrentSets = e.CurrentSets,
                WeightIndex = e.WeightIndex += weightIndexModifier,
                PrimaryExercise = e.PrimaryExercise,
                StartingWeight = e.StartingWeight,
                WeightProgression = e.WeightProgression,
                AttemptsBeforeDeload = e.AttemptsBeforeDeload,
                ExerciseName = e.ExerciseName,
                Username = e.Username,
                Category = e.Category,
                Template = e.Template,
                LiftDay = e.LiftDay,
                LiftOrder = e.LiftOrder,
                LiftWeek = e.LiftWeek += 1,
                ParentId = e.Id,
                CurrentAttempt = e.CurrentAttempt += attemptModifier,
                WorkingWeight = WorkingWeight(e.WorkingWeight, weightIndexModifier, e.WeightIndex, e.WeightProgression),
                EquipmentType = e.EquipmentType,
                UserId = e.UserId
            };

        private static decimal WorkingWeight(decimal workingWeight, int weightIndexModifier, int oldWeightIndex, decimal weightProgression)
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
    }
}
