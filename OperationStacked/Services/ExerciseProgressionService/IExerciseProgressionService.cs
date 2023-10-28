using OperationStacked.Entities;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Services.ExerciseProgressionService
{
    public interface IExerciseProgressionService
    {
        public Task<ExerciseCompletionResult> CompleteExercise(CompleteExerciseRequest request);
        Task<Exercise> UpdateWorkingWeight(UpdateExerciseRequest request);
    }
}
