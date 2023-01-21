using FluentResult;
using Microsoft.EntityFrameworkCore;
using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Repositories
{
    public class ExerciseRetrievalService : IExerciseRetrievalService
    {
        private readonly OperationStackedContext _context;

        public ExerciseRetrievalService(OperationStackedContext context)
        {
            _context = context;
        }
        public async Task<Result<GetWorkoutResult>> GetWorkout(int userId, int week, int day)
        {;
            var p = await GetExercises(userId, week, day);
           
            var lol = new GetWorkoutResult(p
                );

            var t = Result.Create(lol);
            return t;
        }

        private async Task<List<Exercise>> GetExercises(int userId, int week, int day) => _context.Exercises
                .Where(x => x.LiftDay == day &&
                       x.LiftWeek == week &&
                       x.UserId == userId).ToList();

        //=>  _context.Exercises
        //    .Where(x => x.LiftDay == day &&
        //            x.LiftWeek == week &&
        //            x.UserId == userId)
        //             .ToList();
    }
}
