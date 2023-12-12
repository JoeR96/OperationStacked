using OperationStacked.DTOs;
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
    public  Task<IEnumerable<ExerciseHistoryDTO>> GetExerciseHistoryByIds(List<Guid> exerciseIds, int pageIndex, int pageSize);
    public  Task<IEnumerable<ExerciseHistoryDTO>> GetExerciseHistoryByIds(List<Guid> exerciseIds);

}
