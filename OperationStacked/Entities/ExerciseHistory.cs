using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OperationStacked.Entities
{
    public class ExerciseHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime CompletedDate { get; set; }

        [Required]
        public int CompletedSets { get; set; }

        [Required]
        [MaxLength(255)] // you might need to adjust this based on how many reps you expect to store
        public string CompletedReps { get; set; } // comma-separated numbers

        [Required]
        [ForeignKey(nameof(Entities.Exercise))]
        public Guid ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; }

        public Guid? TemplateExerciseId { get; set; }
    
    }
}
