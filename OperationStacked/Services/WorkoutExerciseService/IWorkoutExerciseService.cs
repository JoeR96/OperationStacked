using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;

namespace OperationStacked.Services;

public interface IWorkoutExerciseService
{
    public Task<WorkoutExercise> CreateWorkoutExercise(CreateWorkoutExerciseRequest request);
}
