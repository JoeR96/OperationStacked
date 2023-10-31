namespace OperationStacked.TestLib.Builders;

public class EquipmentStackBuilder
{
    private Entities.EquipmentStack _stack = new ();

    public EquipmentStackBuilder WithDefaultValues()
    {
        _stack = new OperationStacked.Entities.EquipmentStack()
        {
            Id = Guid.NewGuid(),
            StartWeight = 10,
            InitialIncrements = new decimal[]{},
            IncrementValue = 2.5m,
            IncrementCount = 20,
            EquipmentStackKey = "DefaultKey",
        };
        return this;
    }

    public EquipmentStackBuilder WithStartWeight(decimal startWeight)
    {
        _stack.StartWeight = startWeight;
        return this;
    }
    public EquipmentStackBuilder WithIncrementCount(int incrementCount)
    {
        _stack.IncrementCount = incrementCount;
        return this;
    }
    public EquipmentStackBuilder WithInitialIncrements(decimal[] increments)
    {
        _stack.InitialIncrements = increments;
        return this;
    }

    public EquipmentStackBuilder WithIncrementValue(decimal incrementValue)
    {
        _stack.IncrementValue = incrementValue;
        return this;
    }

    public CreateEquipmentStackRequest Adapt()
    {
        return new CreateEquipmentStackRequest
        {

            UserID = _stack.UserID,
            EquipmentStackKey = _stack.EquipmentStackKey
        };
    }
    // Add other builder methods as needed...

    public Entities.EquipmentStack Build() => _stack;

    public OperationStacked.TestLib.CreateEquipmentStackRequest BuildCreateRequest()
    {
        // This method creates a CreateEquipmentStackRequest object from the built EquipmentStack entity.
        return new OperationStacked.TestLib.CreateEquipmentStackRequest
        {
            StartWeight = _stack.StartWeight,
            InitialIncrements = _stack.InitialIncrements,
            IncrementValue = _stack.IncrementValue,
            IncrementCount = _stack.IncrementCount,
            EquipmentStackKey = _stack.EquipmentStackKey,
            UserID = _stack.UserID // Assuming the builder sets this property somewhere before this method is called.
        };
    }

    public EquipmentStackBuilder WithUserId(Guid userId)
    {
        _stack.UserID = userId;
        return this;
    }
}
