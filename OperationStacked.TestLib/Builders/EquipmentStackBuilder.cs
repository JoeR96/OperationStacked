namespace OperationStacked.TestLib.Builders;

public class EquipmentStackBuilder
{
    private EquipmentStack _stack = new ();

    public EquipmentStackBuilder WithDefaultValues()
    {
        _stack = new EquipmentStack()
        {
            Id = Guid.NewGuid(),
            StartWeight = 10,
            InitialIncrements = new decimal?[]{},
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

    // Add other builder methods as needed...

    public EquipmentStack Build() => _stack;
}

