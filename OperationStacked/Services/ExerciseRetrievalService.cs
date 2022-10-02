using FluentResult;
using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Services
{
    public class ExerciseRetrievalService : IExerciseRetrievalService
    {
        private readonly OperationStackedContext _context;

        public ExerciseRetrievalService(OperationStackedContext context)
        {
            _context = context;
        }
        public async Task<Result<GetWorkoutResult>> GetWorkout(int userId, int week, int day)
            => Result.Create(new GetWorkoutResult(await GetExercises(userId, week, day)));

        private async Task<List<Exercise>> GetExercises(int userId, int week, int day)
        {
            return _context.Exercises.Where(x => x.LiftDay == day &&
                        x.LiftWeek == week &&
                        x.UserId == userId)
                         .ToList();
        }


    }
}
