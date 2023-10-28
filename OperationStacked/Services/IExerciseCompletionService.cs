using OperationStacked.Requests;

namespace OperationStacked.Services;

public interface IExerciseCompletionService
{
    public void CompleteExercise(CompleteExerciseRequest request);

}
