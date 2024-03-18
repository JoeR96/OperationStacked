
using Microsoft.EntityFrameworkCore;
using OperationStacked.Data;
using OperationStacked.Entities;


namespace OperationStacked.Repositories.SessionRepository
{
    public class SessionRepository : RepositoryBase, ISessionRepository
    {
        public SessionRepository(IDbContextFactory<OperationStackedContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task InsertSessionAsync(Session session)
        {
            await using var context = _operationStackedContext;
            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync();
        }

        public async Task<Session> GetSessionByIdAsync(Guid sessionId)
        {
            await using var context = _operationStackedContext;
            return await context.Sessions
                .Include(s => s.SessionExercises)
                .ThenInclude(e => e.Sets)
                .FirstOrDefaultAsync(s => s.Id == sessionId);
        }

        public async Task<List<Session>> GetSessionsByUserIdAsync(Guid userId)
        {
            await using var context = _operationStackedContext;
            return await context.Sessions
                .Include(s => s.SessionExercises)
                .ThenInclude(e => e.Sets).ToListAsync();
        }

    

    public async Task UpdateSessionAsync(Session session)
        {
            await using var context = _operationStackedContext;
            context.Sessions.Update(session);
            await context.SaveChangesAsync();
        }

        public async Task DeleteSessionAsync(Guid sessionId)
        {
            await using var context = _operationStackedContext;
            var session = await context.Sessions.FindAsync(sessionId);
            if (session != null)
            {
                context.Sessions.Remove(session);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Session?> GetActiveSessionForUserAsync(Guid userId)
        {
            await using var context = _operationStackedContext;
            return await context.Sessions
                .Where(s => s.UserId == userId)
                .Where(s => s.IsActive == true)
                .Include(s => s.SessionExercises)
                .ThenInclude(e => e.Sets)
                .FirstOrDefaultAsync();
        }        
    }
}
