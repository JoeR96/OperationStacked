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
        Task<EquipmentStackResponse> InsertEquipmentStack(CreateEquipmentStackRequest equipmentStack);
        Task<EquipmentStack> GetEquipmentStack(Guid equipmentStackId);
        Task<bool> DeleteEquipmentStack(Guid equipmentStackId);
        Task<Exercise> UpdateExerciseById(UpdateExerciseRequest request, int weightIndex = -1);
        Task<(IEnumerable<WorkoutExercise> exercises, int totalCount)> GetAllWorkoutExercisesWithCount(Guid userId, int pageIndex, int pageSize);

        Task<List<EquipmentStack>> GetAllEquipmentStacks(Guid userId);
        Task<LinearProgressionExercise> GetLinearProgressionExerciseById(Guid id);
        Task InsertLinearProgressionExercise(LinearProgressionExercise nextExercise);
        Task InsertWorkoutExercise(WorkoutExercise workoutExercise);
        Task InsertExerciseHistory(ExerciseHistory history);
    }
}
