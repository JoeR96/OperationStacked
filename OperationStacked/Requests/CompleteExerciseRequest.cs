namespace OperationStacked.Requests
{
    public class CompleteExerciseRequest
    {
        public Guid Id { get; set; }
        public int[] Reps { get; set; }
        public int Sets { get; set; }

    }
}
