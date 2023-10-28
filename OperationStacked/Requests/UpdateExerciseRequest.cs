namespace OperationStacked.Requests;

public class UpdateExerciseRequest
{
    public int MinimumReps { get; set; }
    public int MaximumReps { get; set; }
    public int Sets { get; set; }
    public Decimal WorkingWeight { get; set; }
    public Guid Id { get; set; }
}