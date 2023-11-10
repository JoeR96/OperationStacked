using OperationStacked.Entities;
using OperationStacked.Requests;

namespace OperationStacked.Services;

public interface IExerciseHistoryService
{
    public Task CompleteExercise(CompleteExerciseRequest request);

    Task<List<ExerciseHistory>> GetExerciseHistoryById(Guid exerciseId);
}
