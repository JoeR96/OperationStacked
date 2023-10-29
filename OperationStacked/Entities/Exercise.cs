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
        public string ExerciseName { get; set; }
        public Category Category { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public Guid UserId { get; set; }

    }
}
