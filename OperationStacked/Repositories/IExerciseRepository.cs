using OperationStacked.Entities;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Repositories
{
    public interface IExerciseRepository
    {
        Task<Exercise> GetExerciseById(Guid id);
        Task<List<Exercise>> GetExercises(Guid userId, int week, int day, bool completed);
        Task InsertExercise(Exercise nextExercise);
        Task UpdateAsync(Exercise exercise);
        Task<bool> DeleteAllExercisesForUser(Guid userId);
        Task<bool> DeleteExercise(Guid exerciseId);
        Task<EquipmentStackResponse> InsertEquipmentStack(CreateEquipmentStackRequest equipmentStack);
        Task<EquipmentStack> GetEquipmentStack(Guid equipmentStackId);
        Task<bool> DeleteEquipmentStack(Guid equipmentStackId);
    }
}