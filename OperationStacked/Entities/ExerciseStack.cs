using System.ComponentModel.DataAnnotations;

namespace OperationStacked.Entities;

public class ExerciseStack
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Decimal StartWeight { get; set; }
    public string? InitialIncrements { get; set; } // Stored as comma separated values
    public Decimal IncrementValue { get; set; }
    public Decimal IncrementCount { get; set; }
    public string Stack { get; set; } // Generated stack stored as comma separated values
    public string StackNAme { get; set; }
}
