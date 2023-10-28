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
            => new GetWorkoutResult(await _exerciseRepository.GetWorkoutExercisesByWeekAndDay(userId, week, day));

        public async Task<GetWorkoutResult> GetAllWorkouts(Guid userId, int pageIndex, int pageSize)
        {
            var (exercises, totalCount) = await _exerciseRepository.GetAllWorkoutExercisesWithCount(userId, pageIndex, pageSize);
            return new GetWorkoutResult(exercises, totalCount);
        }
    }
}
