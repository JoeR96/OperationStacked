using System.ComponentModel.DataAnnotations;

namespace OperationStacked.Entities;

public class Workout
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string WorkoutName { get; set; }
}
