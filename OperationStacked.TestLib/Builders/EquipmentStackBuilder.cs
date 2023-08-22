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
            InitialIncrements = new Decimal?[]{},
            IncrementValue = 2.5m,
            IncrementCount = 20,
            EquipmentStackKey = "DefaultKey",
            UserID = Guid.NewGuid()
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
    public EquipmentStackBuilder WithInitialIncrements(decimal?[] increments)
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
            StartWeight = _stack.StartWeight,
            InitialIncrements = _stack.InitialIncrements,
            IncrementValue = _stack.IncrementValue,
            IncrementCount = _stack.IncrementCount,
            UserID = _stack.UserID,
            EquipmentStackKey = _stack.EquipmentStackKey
        };
    }
    // Add other builder methods as needed...

    public Entities.EquipmentStack Build() => _stack;
}

