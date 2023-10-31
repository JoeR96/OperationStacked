using OperationStacked.Entities;
using OperationStacked.Repositories;
using OperationStacked.Requests;

namespace OperationStacked.Services;

public class WorkoutExerciseService : IWorkoutExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;

    public WorkoutExerciseService(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<WorkoutExercise> CreateWorkoutExercise(CreateWorkoutExerciseRequest request)
    {
        var workoutExercise = new WorkoutExercise
        {
            ExerciseId = request.ExerciseId,
            Template = request.Template,
            LiftDay = request.LiftDay,
            LiftOrder = request.LiftOrder,
            RestTimer = request.RestTimer,

        };



        return workoutExercise;
    }
}
