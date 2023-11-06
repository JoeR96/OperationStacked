using OperationStacked.Entities;
using OperationStacked.Repositories.WorkoutRepository;

namespace OperationStacked.Services.WorkoutService;

public class WorkoutService : IWorkoutService
{
    public IWorkoutRepository _workoutRepository;

    public WorkoutService(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<Workout> CreateWorkout(string workoutName, Guid userId)
    {
        Workout workout = new()
        {
            WorkoutName = workoutName,
            UserId = userId
        };

        return await _workoutRepository.CreateWorkout(workout);
    }
}
