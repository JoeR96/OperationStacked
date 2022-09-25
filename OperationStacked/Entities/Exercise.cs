using OperationStacked.Abstractions;
using OperationStacked.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OperationStacked.Entities
{
    public class Exercise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string ExerciseName { get; set; }
        public string Username { get; set; }
        public string Category { get; set; }
        public ExerciseTemplate Template { get; set; }
        public int LiftDay { get; set; } = 1;
        public int LiftOrder { get; set; } = 1;
        public int LiftWeek { get; set; } = 1;

    }

    public class LinearProgressionExercise : Exercise
    {
        public int MinimumReps { get; set; }
        public int MaximumReps { get; set; }
        public int TargetSets { get; set; }
        public int StartingSets { get; set; }
        public int CurrentSets { get; set; }
        public int WeightIndex { get; set; }
        public bool PrimaryExercise { get; set; } = false;
        public decimal WorkingWeight { get; set; }
        public decimal WeightProgression { get; set; }
        public int AttemptsBeforeDeload { get; set; }
        public int CurrentAttempt { get; set;} = 1;
        public Guid ParentId { get; set; }
    }
}
