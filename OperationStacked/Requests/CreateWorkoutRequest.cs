using OperationStacked.Models;

namespace OperationStacked.Requests
{
    public class CreateWorkoutRequest 
    {
        public int userId { get; set; }
        public List<CreateExerciseModel> ExerciseDaysAndOrders { get; set; }
    }
}
