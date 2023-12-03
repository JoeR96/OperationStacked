using OperationStacked.Entities;
using OperationStacked.Enums;
using OperationStacked.Repositories;
using OperationStacked.Repositories.ExerciseHistoryRepository;
using OperationStacked.Requests;

namespace OperationStacked.Services;

public class ExerciseHistoryService : IExerciseHistoryService
{
    private readonly IExerciseHistoryRepository _exerciseHistoryRepository;

    public ExerciseHistoryService(IExerciseHistoryRepository exerciseHistoryRepository)
    {
        _exerciseHistoryRepository = exerciseHistoryRepository;
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

        await _exerciseHistoryRepository.InsertExerciseHistory(history);
    }

    public async Task<List<ExerciseHistory>> GetExerciseHistoryById(Guid exerciseId)
    {
        return await _exerciseHistoryRepository.GetExerciseHistoryById(exerciseId);
    }

    public async Task<Dictionary<Category, List<ExerciseHistory>>> GetExerciseHistorySortedByCategoryByIds(List<Guid> exerciseIds)
    {
        var exercises = await _exerciseHistoryRepository.GetExerciseHistoriesByIds(exerciseIds);
        var exerciseHistoryByCategory = exercises
            .GroupBy(exercise => exercise.Exercise.Category)
            .ToDictionary(group => group.Key,
                group => group.ToList());

        return exerciseHistoryByCategory;
    }

    public async Task<List<ExerciseHistory>> GetExerciseHistoryByIds(List<Guid> exerciseIds)
    {
        var exercises = await _exerciseHistoryRepository.GetExerciseHistoriesByIds(exerciseIds);
        return exercises;
    }

}
