using OperationStacked.Entities;
using OperationStacked.Repositories;
using OperationStacked.Requests;

namespace OperationStacked.Services.SessionService;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;

    public SessionService(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Session> CreateSessionAsync(Session session)
    {
        await _sessionRepository.InsertSessionAsync(session);
        return session;
    }

    public async Task AddExerciseToSessionAsync(AddExerciseToSessionRequest addExerciseToSessionRequest)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(addExerciseToSessionRequest.SessionId);
        if (session != null)
        {
            session.SessionExercises.Add(new SessionExercise()
            {
                ExerciseId = addExerciseToSessionRequest.ExerciseId,
                SessionId = addExerciseToSessionRequest.SessionId

            });
            await _sessionRepository.UpdateSessionAsync(session);
        }
        else
        {
            throw new KeyNotFoundException("Session not found");
        }
    }

    public async Task AddSetToExerciseAsync(AddSetRequest addSetRequest)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(addSetRequest.SessionId);
        var exercise = session?.SessionExercises.Find(e => e.Id == addSetRequest.ExerciseId);
        if (exercise != null)
        {
            exercise.Sets.Add(new Set()
            {
                Reps = addSetRequest.Reps,
                Weight = addSetRequest.Weight
            });
            await _sessionRepository.UpdateSessionAsync(session);
        }
        else
        {
            throw new KeyNotFoundException("Exercise not found in session");
        }
    }

    public async Task RemoveSetFromExerciseAsync(DeleteSetRequest deleteSetRequest)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(deleteSetRequest.SessionId);
        var exercise = session?.SessionExercises.Find(e => e.Id == deleteSetRequest.ExerciseId);
        if (exercise != null)
        {
            var set = exercise.Sets.Find(s => s.Id == s.Id);
            if (set != null)
            {
                exercise.Sets.Remove(set);
                await _sessionRepository.UpdateSessionAsync(session);
            }
            else
            {
                throw new KeyNotFoundException("Set not found in exercise");
            }
        }
        else
        {
            throw new KeyNotFoundException("Exercise not found in session");
        }
    }

    public async Task<Session> GetSessionByIdAsync(Guid sessionId)
    {
        var session = await _sessionRepository.GetSessionByIdAsync(sessionId);

        return session;
    }

    public async Task<List<Session>> GetSessionsByUserId(Guid userId)
    {
        return await _sessionRepository.GetSessionsByUserIdAsync(userId);
    }

    public async Task<Session?> GetActiveSessionForUser(Guid userId)
    {
        return await _sessionRepository.GetActiveSessionForUserAsync(userId);
    }
}
