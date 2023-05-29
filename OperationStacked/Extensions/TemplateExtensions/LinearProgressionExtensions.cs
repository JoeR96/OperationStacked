using MoreLinq.Extensions;
using OperationStacked.Entities;
using OperationStacked.Migrations;
using OperationStacked.Repositories;
using EquipmentType = OperationStacked.Enums.EquipmentType;

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
        public static LinearProgressionExercise GenerateNextExercise(this LinearProgressionExercise e,decimal workingWeight, int weightIndexModifier, int attemptModifier,IExerciseRepository exerciseRepository = null)
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
                Category = e.Category,
                Template = e.Template,
                LiftDay = e.LiftDay,
                LiftOrder = e.LiftOrder,
                LiftWeek = e.LiftWeek += 1,
                ParentId = e.Id,
                WorkingWeight = workingWeight,
                CurrentAttempt = e.CurrentAttempt += attemptModifier,
                EquipmentType = e.EquipmentType,
                UserId = e.UserId
            };

        // private static decimal WorkingWeight(decimal workingWeight, int weightIndexModifier, int oldWeightIndex,
        //     decimal weightProgression, EquipmentType equipmentType, IExerciseRepository exerciseRepository = null, Guid equipmentStackId = default)
        // {
        //     if (equipmentType is EquipmentType.Barbell or EquipmentType.SmithMachine)
        //     {
        //         if(weightIndexModifier > 0)
        //         {
        //             return workingWeight += weightProgression;
        //         }
        //         else if(weightIndexModifier == 0)
        //         {
        //             return workingWeight;
        //         }
        //         else
        //         {
        //             return workingWeight -= weightProgression;
        //         }
        //     }
        //
        //     if (equipmentType == EquipmentType.Dumbbell)
        //     {
        //         return workingWeight > 9 ? workingWeight += 2 : workingWeight += 1;
        //     }
        //
        //     if (equipmentType is EquipmentType.Cable or EquipmentType.Machine)
        //     {
        //         var stack = exerciseRepository.GetEquipmentStack(equipmentStackId).Result.GenerateStack();
        //         int index = Array.IndexOf(stack, workingWeight);
        //         index += 1;
        //         return (decimal)stack[index];
        //
        //     }
        //     throw new NotImplementedException("EquipmentType is not supported");
        // }

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
