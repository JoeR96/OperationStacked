using OperationStacked.Entities;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Services.ExerciseProgressionService
{
    public interface IWorkoutExerciseProgressionService
    {
        public Task<ExerciseCompletionResult> CompleteExercise(CompleteExerciseRequest request);
        Task<LinearProgressionExercise> UpdateWorkingWeight(UpdateExerciseRequest request);
        Task<List<ExerciseCompletionResult>> CompleteExercises(List<CompleteExerciseRequest> request);
    }
}
