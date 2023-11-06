using OperationStacked.Enums;

namespace OperationStacked.Models
{
    public class CreateExerciseRequest
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string ExerciseName { get; set; } = string.Empty;
        [System.ComponentModel.DataAnnotations.Required]
        public Category Category { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public Guid UserId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public EquipmentType EquipmentType { get; set; }

    }
}
