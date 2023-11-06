using OperationStacked.Entities;
using OperationStacked.Repositories;
using OperationStacked.Requests;

namespace OperationStacked.Services;

public class ExerciseHistoryService : IExerciseHistoryService
{
    private readonly IExerciseRepository _exerciseRepository;

    public ExerciseHistoryService(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }


    public async Task CompleteExercise(CompleteExerciseRequest request)
    {
        var history = new ExerciseHistory()
        {
            CompletedDate = DateTime.Now,
            CompletedReps = string.Join(",", request.Reps),
            CompletedSets = request.Sets,
            ExerciseId = request.ExerciseId,
        };

        if (request.LinearProgressionExerciseId != Guid.Empty)
        {
            history.TemplateExerciseId = request.LinearProgressionExerciseId;
        }
        else
        {
            history.TemplateExerciseId = Guid.Empty;;
        }

        await _exerciseRepository.InsertExerciseHistory(history);
    }
}
