using Microsoft.EntityFrameworkCore;
using OperationStacked.Data;
using OperationStacked.Entities;

namespace OperationStacked.Repositories.ExerciseHistoryRepository;

public class ExerciseHistoryRepository : RepositoryBase, IExerciseHistoryRepository
{
    public async Task InsertExerciseHistory(ExerciseHistory history)
    {
        using var context = _operationStackedContext;
        await context.ExerciseHistory.AddAsync(history);
        await context.SaveChangesAsync();
    }

    public async Task<List<ExerciseHistory>> GetExerciseHistoryById(Guid exerciseId)
    {
        return await _operationStackedContext.ExerciseHistory.Where(eh => eh.Exercise.Id == exerciseId).ToListAsync();
    }

    public async Task<List<ExerciseHistory>> GetExerciseHistoriesByIds(List<Guid> exerciseIds)
    {
        return await _operationStackedContext.ExerciseHistory
            .Where(eh => exerciseIds.Contains(eh.Exercise.Id))
            .Include(eh => eh.Exercise)
            .ToListAsync();
    }

    public async Task DeleteExerciseHistoryById(Guid id)
    {
        Guid.Parse(id.ToString());

        using var context = _operationStackedContext;

        context.ExerciseHistory.RemoveRange(
            context.ExerciseHistory.Where(eh => eh.Id == id)
        );

        await context.SaveChangesAsync();
    }


    public ExerciseHistoryRepository(IDbContextFactory<OperationStackedContext> contextFactory) : base(contextFactory)
    {
    }
}
