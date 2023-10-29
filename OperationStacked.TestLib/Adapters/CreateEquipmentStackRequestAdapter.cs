namespace OperationStacked.TestLib.Adapters;

public static class CreateEquipmentStackRequestAdapter
{
    public static Requests.CreateEquipmentStackRequest Adapt(
       this  CreateEquipmentStackRequest source)
    {
        return new OperationStacked.Requests.CreateEquipmentStackRequest
        {
            InitialIncrements = source.InitialIncrements,
            IncrementValue = source.IncrementValue,
            IncrementCount = source.IncrementCount,
            EquipmentStackKey = source.EquipmentStackKey,
            UserID = source.UserID
        };
    }
}
