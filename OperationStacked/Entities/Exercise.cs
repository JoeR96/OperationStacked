using OperationStacked.Abstractions;
using OperationStacked.Enums;
using OperationStacked.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OperationStacked.Entities
{
    public abstract class Exercise : IExercise
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string ExerciseName { get; set; }
        public string Username { get; set; }
        public string Category { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public ExerciseTemplate Template { get; set; }
        public int LiftDay { get; set; } = 1;
        public int LiftOrder { get; set; } = 1;
        public int LiftWeek { get; set; } = 1;
        public int UserId { get; set; }
        public decimal WorkingWeight { get; set; }
        public Guid ParentId { get; set; }

    }
}
