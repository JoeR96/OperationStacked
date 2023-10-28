using OperationStacked.Entities;
using OperationStacked.Repositories;
using OperationStacked.Requests;

namespace OperationStacked.Services;

public class ExerciseCompletionService : IExerciseCompletionService
{
    private IExerciseRepository _exerciseRepository;

    public ExerciseCompletionService(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }


    public void CompleteExercise(CompleteExerciseRequest request)
    {
        var history = new ExerciseHistory()
        {
            CompletedDate = DateTime.Now,
            CompletedReps = string.Join(",", request.Reps),
            CompletedSets = request.Sets,
            ExerciseId = request.ExerciseId,
            TemplateExerciseId = request.ExerciseTemplateId
        };

        _exerciseRepository.InsertExerciseHistory(history);
        
    }
}
