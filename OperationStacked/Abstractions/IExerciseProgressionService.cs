using FluentResult;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Abstractions
{
    public interface IExerciseProgressionService
    {
        public Task<Result<ExerciseCompletionResult>> CompleteExercise(CompleteExerciseRequest request);
    }
}
