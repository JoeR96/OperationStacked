using System.ComponentModel.DataAnnotations;

namespace OperationStacked.Models
{
    public class CreateExerciseModel
    {
        [Required]
        public string ExerciseName { get; set; } = string.Empty;
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Category { get; set; } = string.Empty;
        [Required]
        public string Template { get; set; } = string.Empty;
        [Required]
        public int LiftDay { get; set; }
        [Required]
        public int LiftOrder { get; set; }
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
        public int UserId { get; set; }
    }
}
