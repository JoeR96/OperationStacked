namespace OperationStacked.Requests;

public class AddSetRequest
{
    public Guid SessionId { get; set; }
    public Guid ExerciseId { get; set; }
    public int Reps { get; set; }
    public double Weight { get; set; }
}
