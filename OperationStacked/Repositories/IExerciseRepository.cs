using OperationStacked.Entities;

namespace OperationStacked.Repositories
{
    public interface IExerciseRepository
    {
        Task<Exercise> GetExerciseById(Guid id);
        Task<List<Exercise>> GetExercises(string userId, int week, int day, bool completed);
        Task InsertExercise(Exercise nextExercise);
        Task UpdateAsync(Exercise exercise);
    }
}