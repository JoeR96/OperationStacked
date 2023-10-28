using OperationStacked.Entities;
using OperationStacked.Models;

namespace OperationStacked.Services;

public interface IWorkoutExerciseService
{
    public Task<WorkoutExercise> CreateWorkoutExercise(CreateExerciseModel request);
}
