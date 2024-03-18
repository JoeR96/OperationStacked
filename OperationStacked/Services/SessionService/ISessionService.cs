using OperationStacked.Entities;
using OperationStacked.Requests;

namespace OperationStacked.Services
{
    public interface ISessionService
    {
        Task<Session> CreateSessionAsync(Session session);
        Task AddExerciseToSessionAsync(AddExerciseToSessionRequest addExerciseToSessionRequest);
        Task AddSetToExerciseAsync(AddSetRequest addSetRequest);
        Task RemoveSetFromExerciseAsync(DeleteSetRequest deleteSetRequest);
        Task<Session?> GetSessionByIdAsync(Guid sessionId);
        Task<List<Session>> GetSessionsByUserId(Guid sessionId);
    }
}
