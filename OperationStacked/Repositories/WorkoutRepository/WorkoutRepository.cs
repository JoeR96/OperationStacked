using OperationStacked.Data;
using OperationStacked.Entities;

namespace OperationStacked.Repositories.WorkoutRepository;

public class WorkoutRepository : IWorkoutRepository
{
    public OperationStackedContext  _operationStackedContext;

    public WorkoutRepository(OperationStackedContext operationStackedContext)
    {
        _operationStackedContext = operationStackedContext;
    }

    public async Task<Workout> CreateWorkout(Workout workout)
    {
       _operationStackedContext.Workouts.Add(workout);
       await _operationStackedContext.SaveChangesAsync();

       return workout;
    }

    public async Task<List<Workout>> GetWorkouts(Guid userId)
    {
        Guid.Parse(userId.ToString());
       return _operationStackedContext.Workouts.Where(wo => wo.UserId == userId).ToList();
    }
}
