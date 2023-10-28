using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Repositories;

namespace OperationStacked.Services;

public class WorkoutExerciseService : IWorkoutExerciseService
{
    private IExerciseRepository _exerciseRepository;

    public WorkoutExerciseService(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<WorkoutExercise> CreateWorkoutExercise(CreateExerciseModel request)
    {
        var workoutExercise = new WorkoutExercise
        {
            ExerciseId = request.ExerciseId,
            Template = request.Template,
            LiftDay = request.LiftDay,
            LiftOrder = request.LiftOrder,
            RestTimer = request.RestTimer,
        };

        _exerciseRepository.InsertWorkoutExercise(workoutExercise);

        return workoutExercise;
    }
}
