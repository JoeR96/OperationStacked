using OperationStacked.Models;

namespace OperationStacked.Requests
{
    public class CreateWorkoutRequest 
    {
        public Guid UserId { get; set; }
        public string WorkoutName { get; set; }
        public List<CreateLinearProgressionExerciseRequest> Exercises { get; set; }
    }
}
