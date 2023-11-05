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
            Sets = request.Sets,
            AttemptsBeforeDeload = request.AttemptsBeforeDeload,
            WeightProgression = request.WeightProgression,
            MinimumReps = request.MinimumReps,
            MaximumReps = request.MaximumReps,
            ExerciseId = request.ExerciseId,
            Template = request.Template,
            LiftDay = request.LiftDay,
            LiftOrder = request.LiftOrder,
            RestTimer = request.RestTimer,
            WorkoutId = request.WorkoutId
        };

        return workoutExercise;
    }
}
