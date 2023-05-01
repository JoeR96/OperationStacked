using OperationStacked.Enums;
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
        public ExerciseTemplate Template { get; set; }
        [Required]
        public EquipmentType EquipmentType { get; set; }
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
        public decimal StartingWeight { get; set; }
        public decimal WeightProgression { get; set; }
        public int AttemptsBeforeDeload { get; set; }
        public string UserId { get; set; }
        public bool PrimaryLift { get; set; }
        public A2SBlocks Block { get; set; }
        public decimal TrainingMax { get; set; }
        public decimal Intensity { get; set; }
        public int Sets { get; set; }
        public int RepsPerSet { get; set; }
        public int Week { get; set; }
        public Guid ParentId { get; set; }
    }
}
