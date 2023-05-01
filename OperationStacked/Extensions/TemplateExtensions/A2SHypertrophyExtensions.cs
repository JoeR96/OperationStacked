using OperationStacked.Entities;
using OperationStacked.Migrations;

namespace OperationStacked.Extensions.TemplateExtensions
{
    public static class A2SHypertrophyExtensions
    {
        public static A2SHypertrophyExercise GenerateNextExercise(this A2SHypertrophyExercise e,
            decimal trainingMax,
            decimal workingWeight,
            Enums.A2SBlocks block,
            int week,
            int amrapRepTarget,
            decimal intensity,
            int repsPerSet,
            int a2sWeek,
            string userId)
           => new A2SHypertrophyExercise
           {
               TrainingMax = trainingMax,
               PrimaryLift = e.PrimaryLift,
               Block = block,
               AmrapRepTarget = amrapRepTarget,
               AmrapRepResult = 0,
               Intensity = intensity,
               Sets = e.Sets,
               RepsPerSet = repsPerSet,
               ExerciseName = e.ExerciseName,
               Username = e.Username,
               Category = e.Category,
               Template = e.Template,
               LiftDay = e.LiftDay,
               LiftOrder = e.LiftOrder,
               LiftWeek = week += 1,
               Week = a2sWeek,
               ParentId = e.Id,
               WorkingWeight = workingWeight,
               EquipmentType = e.EquipmentType,
               RoundingValue = e.RoundingValue,
               UserId = userId
           };

    }
}
