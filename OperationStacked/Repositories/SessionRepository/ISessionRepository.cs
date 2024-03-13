using OperationStacked.Entities;

namespace OperationStacked.Repositories
{
    public interface ISessionRepository
    {
        Task InsertSessionAsync(Session session);
        Task<Session> GetSessionByIdAsync(Guid sessionId);
        Task<List<Session>> GetSessionsByUserIdAsync(string userId);
        Task UpdateSessionAsync(Session session);
        Task DeleteSessionAsync(Guid sessionId);
    }
}
