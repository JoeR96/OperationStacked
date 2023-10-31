using System.ComponentModel.DataAnnotations.Schema;
using OperationStacked.Enums;
using OperationStacked.Models;

namespace OperationStacked.Entities
{
    [Table("LinearProgressionExercise")]

    public class LinearProgressionExercise 
    {
        [ForeignKey("WorkoutExercise")]
        public Guid WorkoutExerciseId { get; set; }
        public virtual WorkoutExercise WorkoutExercise { get; set; }
        public Guid Id { get; set; }
        public int CurrentAttempt { get; set; } = 0;
        public Guid ParentId { get; set; } = new Guid();
        public int LiftWeek { get; set; } = 1;
        public decimal WorkingWeight { get; set; }
        public int WeightIndex { get; set; }
    }
}
