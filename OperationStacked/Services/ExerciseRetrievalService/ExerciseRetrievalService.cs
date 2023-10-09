using OperationStacked.Repositories;
using OperationStacked.Response;

namespace OperationStacked.Services.ExerciseRetrievalService
{
    public class ExerciseRetrievalService : IExerciseRetrievalService
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseRetrievalService(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }
        public async Task<GetWorkoutResult> GetWorkout(Guid userId, int week, int day, bool completed)
        => new GetWorkoutResult(await _exerciseRepository.GetExercises(userId, week, day, completed));

        public async Task<GetWorkoutResult> GetAllWorkouts(Guid userId)
            => new GetWorkoutResult(await _exerciseRepository.GetAllExercises(userId));
    }
}
