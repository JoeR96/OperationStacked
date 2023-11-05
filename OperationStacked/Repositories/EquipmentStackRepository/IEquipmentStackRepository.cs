using OperationStacked.Entities;
using OperationStacked.Requests;
using OperationStacked.Response;
namespace OperationStacked.Repositories;
public interface IEquipmentStackRepository
{
    Task<EquipmentStackResponse> InsertEquipmentStack(CreateEquipmentStackRequest equipmentStack);
    Task<EquipmentStack> GetEquipmentStack(Guid equipmentStackId);
    Task<List<EquipmentStack>> GetAllEquipmentStacks(Guid userId);
    Task<bool> DeleteEquipmentStack(Guid equipmentStackId);
}
