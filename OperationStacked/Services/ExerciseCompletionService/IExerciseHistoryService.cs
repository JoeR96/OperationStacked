using OperationStacked.Entities;
using OperationStacked.Enums;
using OperationStacked.Requests;

namespace OperationStacked.Services;

public interface IExerciseHistoryService
{
    public Task CompleteExercise(CompleteExerciseRequest request);

    Task<List<ExerciseHistory>> GetExerciseHistoryById(Guid exerciseId);

    public  Task<Dictionary<Category, List<ExerciseHistory>>> GetExerciseHistorySortedByCategoryByIds(
        List<Guid> exerciseIds);
    Task<List<ExerciseHistory>> GetExerciseHistoryByIds(List<Guid> exerciseIds);
}
