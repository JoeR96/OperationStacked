using OperationStacked.Models;

namespace OperationStacked.Requests
{
    public class CreateWorkoutRequest 
    {
        public string userId { get; set; }
        public List<CreateExerciseModel> ExerciseDaysAndOrders { get; set; }
    }
}
