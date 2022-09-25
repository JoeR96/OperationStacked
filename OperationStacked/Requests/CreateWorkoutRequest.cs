using OperationStacked.Models;

namespace OperationStacked.Requests
{
    public class CreateWorkoutRequest 
    {
        public int userId;
        public List<CreateExerciseModel> ExerciseDaysAndOrders { get; set; }
    }
}
