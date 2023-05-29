using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Services.ExerciseCreationService
{
    public interface IExerciseCreationService
    {

        Task<WorkoutCreationResult> CreateWorkout(CreateWorkoutRequest request);
    }
}
