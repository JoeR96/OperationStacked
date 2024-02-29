using OperationStacked.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OperationStacked.Entities
{
    public class Exercise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string ExerciseName { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public EquipmentType EquipmentType { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public virtual ICollection<ExerciseHistory> ExerciseHistories { get; set; }
    }
}
