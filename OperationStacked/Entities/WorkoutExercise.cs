using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OperationStacked.Models;

namespace OperationStacked.Entities;

public class WorkoutExercise
{
    [Key]
    public Guid Id { get; set; }
    public Guid WorkoutId { get; set; }
    [ForeignKey("Exercise")]
    public Guid ExerciseId { get; set; }    public virtual Exercise Exercise { get; set; }
    public virtual ICollection<LinearProgressionExercise> LinearProgressionExercises { get; set; }
    public ExerciseTemplate Template { get; set; }
    public int LiftDay { get; set; }
    public int LiftOrder { get; set; }
    public bool Completed { get; set; }
    public int RestTimer { get; set; }
    public int MinimumReps { get; set; }
    public int MaximumReps { get; set; }
    public int Sets { get; set; }
    public decimal WeightProgression { get; set; }
    public int AttemptsBeforeDeload { get; set; }
    public Guid EquipmentStackId { get; set; }

}
