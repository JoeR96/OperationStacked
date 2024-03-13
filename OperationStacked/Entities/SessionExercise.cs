using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OperationStacked.Entities;

public class SessionExercise
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [ServiceStack.DataAnnotations.ForeignKey(typeof(Exercise))]
    [ForeignKey("SessionId")]
    public virtual Session Session { get; set; }
    public Guid SessionId { get; set; }
    public Guid ExerciseId { get; set; }
    public virtual Exercise Exercise { get; set; }
    public List<Set> Sets { get; set; } = new ();

}
