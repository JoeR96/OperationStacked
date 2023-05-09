using OperationStacked.Data;
using OperationStacked.Entities;
using ServiceStack;

namespace OperationStacked.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly OperationStackedContext _context
            ;
        public ExerciseRepository(OperationStackedContext context)
        {
            _context = context;
        }

        public async Task<List<Exercise>> GetExercises(string userId, int week, int day, bool completed) => completed ? _context.Exercises.Where(x => x.LiftDay == day &&
               x.LiftWeek == week &&
               x.UserId == userId 
        ).ToList() : _context.Exercises.Where(x => x.LiftDay == day &&
               x.LiftWeek == week &&
               x.UserId == userId &&
               x.Completed == false
        ).ToList();
        public async Task<Exercise> GetExerciseById(Guid id) => _context.Exercises
        .FirstOrDefault(x => x.Id == id);

        public async Task InsertExercise(Exercise nextExercise)
        {
            Console.WriteLine(nextExercise.UserId);
            await _context.Exercises.AddAsync(nextExercise);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Exercise exercise)
        {
            _context.Exercises.Update(exercise);
            await _context.SaveChangesAsync();
        }
    }
}
