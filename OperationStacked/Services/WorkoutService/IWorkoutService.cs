using OperationStacked.Entities;

namespace OperationStacked.Services.WorkoutService;

public interface IWorkoutService
{
    Task<Workout> CreateWorkout(string workoutName, Guid userId);
}
