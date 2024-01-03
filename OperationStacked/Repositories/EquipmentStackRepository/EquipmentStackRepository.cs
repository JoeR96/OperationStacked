using Microsoft.EntityFrameworkCore;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Repositories;

public class EquipmentStackRepository : RepositoryBase, IEquipmentStackRepository
{
    public EquipmentStackRepository(IDbContextFactory<OperationStackedContext> contextFactory)
        : base(contextFactory)
    {
    }
    public async Task<EquipmentStackResponse> InsertEquipmentStack(CreateEquipmentStackRequest equipmentStack)
    {
        using var context = _operationStackedContext;

        var e = new EquipmentStack()
        {
            IncrementCount = equipmentStack.IncrementCount,
            Name = equipmentStack.EquipmentStackKey,
            IncrementValue = equipmentStack.IncrementValue,
            InitialIncrements = equipmentStack.InitialIncrements,
            UserID = equipmentStack.UserID,
            StartWeight = equipmentStack.StartWeight,

        };
        await context.EquipmentStacks.AddAsync(e);
        await context.SaveChangesAsync();
        var _ = await  context.EquipmentStacks.FirstOrDefaultAsync();
        return new EquipmentStackResponse(e);
    }

    public async Task<EquipmentStack> GetEquipmentStack(Guid equipmentStackId)
    {
        try
        {
            return await _operationStackedContext.EquipmentStacks.FirstOrDefaultAsync(x => x.Id == equipmentStackId);

        }
        catch (Exception e)
        {
            // Log or print the exception
            Console.WriteLine(e);
            throw; // rethrowing the exception
        }
    }

    public async Task<List<EquipmentStack>> GetAllEquipmentStacks(Guid userId)
    {
        try
        {
            return await _operationStackedContext.EquipmentStacks.Where(x => x.UserID == userId).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> DeleteEquipmentStack(Guid equipmentStackId)
    {
        if (equipmentStackId == null)
        {
            throw new ArgumentNullException(nameof(equipmentStackId));
        }

        var equipmentStack = await _operationStackedContext.EquipmentStacks.FirstOrDefaultAsync(x => x.Id == equipmentStackId);

        if (equipmentStack == null)
        {
            throw new KeyNotFoundException($"No EquipmentStack was found with the ID {equipmentStackId}");
        }

        _operationStackedContext.EquipmentStacks.Remove(equipmentStack);
        var saveResult = await _operationStackedContext.SaveChangesAsync();

        return saveResult > 0;
    }
}
