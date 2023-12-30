using OperationStacked.Entities;

namespace OperationStacked.Repositories.ExerciseHistoryRepository;

public interface IExerciseHistoryRepository
{
    Task InsertExerciseHistory(ExerciseHistory history);

    Task<List<ExerciseHistory>> GetExerciseHistoryById(Guid exerciseId);
    Task<List<ExerciseHistory>> GetExerciseHistoriesByIds(List<Guid> exerciseIds);
    Task DeleteExerciseHistoryById(Guid exerciseId);
}
