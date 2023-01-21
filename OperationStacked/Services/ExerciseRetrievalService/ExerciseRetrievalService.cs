using FluentResult;
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
        public async Task<Result<GetWorkoutResult>> GetWorkout(int userId, int week, int day)
        => Result.Create(new GetWorkoutResult(await _exerciseRepository.GetExercises(userId, week, day)));


    }
}
