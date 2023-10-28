using System.ComponentModel.DataAnnotations.Schema;
using OperationStacked.Enums;
using OperationStacked.Models;

namespace OperationStacked.Entities
{
    [Table("LinearProgressionExercise")]

    public class LinearProgressionExercise 
    {
        public Guid WorkoutExerciseId { get; set; }
        public virtual WorkoutExercise WorkoutExercise { get; set; }
        public Guid Id { get; set; }
        public int MinimumReps { get; set; }
        public int MaximumReps { get; set; }
        public int Sets { get; set; }
        public decimal WeightProgression { get; set; }
        public int AttemptsBeforeDeload { get; set; }
        public int CurrentAttempt { get; set; } = 0;
        public Guid ParentId { get; set; } = new Guid();
        public int LiftWeek { get; set; } = 1;
        public decimal WorkingWeight { get; set; }
        public int WeightIndex { get; set; }
        public Guid EquipmentStackId { get; set; }


    }
}
