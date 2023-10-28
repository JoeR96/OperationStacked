using OperationStacked.Models;
using ServiceStack.DataAnnotations;

namespace OperationStacked.Entities;

public class WorkoutExercise
{
    [PrimaryKey]
    public Guid Id { get; set; }
    public Guid WorkoutId { get; set; }
    public Guid ExerciseId { get; set; }
    public virtual Exercise Exercise { get; set; }
    [ForeignKey(typeof(Guid))]
    public virtual LinearProgressionExercise LinearProgressionExercise { get; set; }    public ExerciseTemplate Template { get; set; }
    public int LiftDay { get; set; }
    public int LiftOrder { get; set; }
    public bool Completed { get; set; }
    public int RestTimer { get; set; }

}
