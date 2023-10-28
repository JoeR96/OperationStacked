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
        public string Category { get; set; }
        public string? CompletedReps {get; set;}
        public EquipmentType EquipmentType { get; set; }
        public ExerciseTemplate Template { get; set; }
        public int LiftDay { get; set; } = 1;
        public int LiftOrder { get; set; } = 1;
        public int LiftWeek { get; set; } = 1;
        public Guid UserId { get; set; }
        public decimal WorkingWeight { get; set; }
        public Guid ParentId { get; set; }
        public bool Completed { get; set; }
        public int RestTimer { get; set; }
        public Guid EquipmentStackId { get; set; }
        public int WeightIndex { get; set; }


    }
}
