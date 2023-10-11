using OperationStacked.Entities;
using OperationStacked.Response;

namespace OperationStacked.Services.ExerciseRetrievalService
{
    public interface IExerciseRetrievalService
    {
        public Task<GetWorkoutResult> GetWorkout(Guid userid, int week, int day, bool completed);
        public Task<GetWorkoutResult> GetAllWorkouts(Guid userId, int pageIndex, int pageSize);
    }
}
