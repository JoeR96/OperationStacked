using OperationStacked.DTOs;
using OperationStacked.Entities;
using OperationStacked.Enums;
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
            WorkingWeight = request.WorkingWeight,
        };

        if (request.DummyTime != DateTime.MinValue){

            history.CompletedDate = request.DummyTime;

        }
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

    public async Task<IEnumerable<ExerciseHistoryDTO>> GetExerciseHistoryByIds(List<Guid> exerciseIds, int pageIndex, int pageSize)
    {
        var exercisesHistories = await _exerciseHistoryRepository.GetExerciseHistoriesByIds(exerciseIds);

        // Apply pagination
        var paginatedHistories = exercisesHistories
            .Skip(pageIndex * pageSize)
            .Take(pageSize);

        var exercisesHistoryDTOs = paginatedHistories.Select(eh => new ExerciseHistoryDTO
        {
            Id = eh.Id,
            CompletedDate = eh.CompletedDate,
            CompletedSets = eh.CompletedSets,
            CompletedReps = eh.CompletedReps,
            ExerciseId = eh.ExerciseId,
            TemplateExerciseId = eh.TemplateExerciseId,
            WorkingWeight= eh.WorkingWeight,
            Exercise = eh.Exercise != null ? new ExerciseDTO
            {
                Id = eh.Exercise.Id,
                ExerciseName = eh.Exercise.ExerciseName,
                Category = eh.Exercise.Category,
                EquipmentType = eh.Exercise.EquipmentType
            } : null
        }).ToList();

        return exercisesHistoryDTOs;
    }
    public async Task<IEnumerable<ExerciseHistoryDTO>> GetExerciseHistoryByIds(List<Guid> exerciseIds)
    {
        var exercisesHistories = await _exerciseHistoryRepository.GetExerciseHistoriesByIds(exerciseIds);

        // Apply pagination

        var exercisesHistoryDTOs = exercisesHistories.Select(eh => new ExerciseHistoryDTO
        {
            Id = eh.Id,
            CompletedDate = eh.CompletedDate,
            CompletedSets = eh.CompletedSets,
            CompletedReps = eh.CompletedReps,
            ExerciseId = eh.ExerciseId,
            TemplateExerciseId = eh.TemplateExerciseId,
            WorkingWeight= eh.WorkingWeight,
            Exercise = eh.Exercise != null ? new ExerciseDTO
            {
                Id = eh.Exercise.Id,
                ExerciseName = eh.Exercise.ExerciseName,
                Category = eh.Exercise.Category,
                EquipmentType = eh.Exercise.EquipmentType
            } : null
        }).ToList();

        return exercisesHistoryDTOs;
    }

    public async Task DeleteExerciseHistoryById(Guid exerciseId)
    {
        await _exerciseHistoryRepository.DeleteExerciseHistoryById(exerciseId);
    }
}
