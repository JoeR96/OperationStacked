using FluentResult;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Abstractions
{
    public interface IExerciseCreationService
    {
        
        Task<Result<WorkoutCreationResult>> CreateWorkout(CreateWorkoutRequest request);
    }
}
