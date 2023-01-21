using FluentResult;
using OperationStacked.Entities;
using OperationStacked.Response;

namespace OperationStacked.Repositories
{
    public interface IExerciseRetrievalService
    {
        public Task<Result<GetWorkoutResult>> GetWorkout(int userid, int week, int day);
    }
}
