namespace OperationStacked.Requests;

public class AddExerciseToSessionRequest
{
    public Guid SessionId { get; set; }
    public Guid ExerciseId { get; set; }
}
