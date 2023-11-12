using OperationStacked.Entities;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Repositories
{
    public interface IExerciseRepository
    {
        Task<Exercise> GetExerciseById(Guid id);
        Task<List<WorkoutExercise>> GetWorkoutExercisesByWeekAndDay(Guid userId, int week, int day);
        Task InsertExercise(Exercise nextExercise);
        Task UpdateAsync(Exercise exercise);
        Task<bool> DeleteAllExercisesForUser(Guid userId);
        Task<bool> DeleteExercise(Guid exerciseId);
        Task<Exercise> UpdateExerciseById(UpdateExerciseRequest request, int weightIndex = -1);
        Task<(IEnumerable<WorkoutExercise> exercises, int totalCount)> GetAllWorkoutExercisesWithCount(Guid userId, int pageIndex, int pageSize);
        Task<LinearProgressionExercise> GetLinearProgressionExerciseByIdAsync(Guid id);
        Task InsertLinearProgressionExercise(LinearProgressionExercise nextExercise);
        Task InsertWorkoutExercise(WorkoutExercise workoutExercise);
        Task<WorkoutExercise> GetWorkoutExerciseById(Guid requestWorkoutExerciseId);
        Task<List<Exercise>> GetAllExercisesByUserId(Guid userId);
    }
}
