using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OperationStacked.Entities;

public class Set
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [ServiceStack.DataAnnotations.ForeignKey(typeof(SessionExercise))]
    public int Reps { get; set; }
    public double Weight { get; set; }
}
