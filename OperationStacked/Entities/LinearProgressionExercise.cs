using System.ComponentModel.DataAnnotations.Schema;
using OperationStacked.Enums;

namespace OperationStacked.Entities
{
    [Table("LinearProgressionExercise")]

    public class LinearProgressionExercise : Exercise
    {
        public int MinimumReps { get; set; }
        public int MaximumReps { get; set; }
        public int Sets { get; set; }
        public int WeightIndex { get; set; }
        public bool PrimaryExercise { get; set; } = false;
        public decimal WeightProgression { get; set; }
        public int AttemptsBeforeDeload { get; set; }
        public int FailedAttempts { get; set; } = 0;
        public Guid EquipmentStackId { get; set; }
        public int CurrentAttempt { get; set; }

        

    }
}
