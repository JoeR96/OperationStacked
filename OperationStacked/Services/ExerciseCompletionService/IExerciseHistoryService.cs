using OperationStacked.Requests;

namespace OperationStacked.Services;

public interface IExerciseHistoryService
{
    public Task CompleteExercise(CompleteExerciseRequest request);

}
