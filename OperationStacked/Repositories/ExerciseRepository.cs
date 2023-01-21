using OperationStacked.Data;
using OperationStacked.Entities;

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

        public async Task<List<Exercise>> GetExercises(int userId, int week, int day) => _context.Exercises
        .Where(x => x.LiftDay == day &&
               x.LiftWeek == week &&
               x.UserId == userId).ToList();
        public async Task<Exercise> GetExerciseById(Guid id) => _context.Exercises
        .FirstOrDefault(x => x.Id == id);

        public async Task InsertExercise(Exercise nextExercise)
        {
            await _context.Exercises.AddAsync(nextExercise);
            await _context.SaveChangesAsync();
        }
    }
}
