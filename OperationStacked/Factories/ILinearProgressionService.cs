using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;
using CreateExerciseRequest = OperationStacked.Models.CreateExerciseRequest;

namespace OperationStacked.Factories;

public interface ILinearProgressionService
{
    public Task<LinearProgressionExercise> CreateLinearProgressionExercise(
        CreateLinearProgressionExerciseRequest createExerciseRequest,
        WorkoutExercise workoutExercise
    );
    Task<(LinearProgressionExercise, ExerciseCompletedStatus)> ProgressExercise(CompleteExerciseRequest request);

    decimal CreateStack(Guid exerciseParentId,
        decimal workingWeight, int startIndex,
        EquipmentStack stack);
}
