using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OperationStacked.Entities;

public class Session
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime SessionStartDateTime { get; set; }
    public string SessionName { get; set; }
    public List<SessionExercise> SessionExercises { get; set; } = new();
    public bool IsActive { get; set; }
}
