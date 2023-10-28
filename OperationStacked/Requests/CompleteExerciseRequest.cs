using OperationStacked.Models;

namespace OperationStacked.Requests
{
    public class CompleteExerciseRequest
    {
        public Guid ExerciseId { get; set; }
        public Guid ExerciseTemplateId { get; set; }
        public int[] Reps { get; set; }
        public int Sets { get; set; }
        public ExerciseTemplate Template { get; set; }
    }
}
