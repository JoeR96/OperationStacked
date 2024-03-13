namespace OperationStacked.Requests;

public class DeleteSetRequest
{
    public Guid SessionId { get; set; }
    public Guid ExerciseId { get; set; }
    public Guid SetId { get; set; }
}
