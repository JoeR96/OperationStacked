using FluentResult;
using OperationStacked.Entities;
using OperationStacked.Response;

namespace OperationStacked.Services.ExerciseRetrievalService
{
    public interface IExerciseRetrievalService
    {
        public Task<Result<GetWorkoutResult>> GetWorkout(int userid, int week, int day, bool completed);
    }
}
