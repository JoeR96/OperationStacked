using FluentResult;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Services.ExerciseCreationService
{
    public interface IExerciseCreationService
    {

        Task<Result<WorkoutCreationResult>> CreateWorkout(CreateWorkoutRequest request);
    }
}
