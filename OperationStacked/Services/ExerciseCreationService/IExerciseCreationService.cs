using OperationStacked.Entities;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Services.ExerciseCreationService
{
    public interface IExerciseCreationService
    {

        Task<WorkoutCreationResult> CreateWorkout(CreateWorkoutRequest request);
        Task<List<Exercise>> CreateExercises(List<CreateExerciseRequest> requests);
        Task<Exercise> CreateExercise(CreateExerciseRequest requests);


    }
}
