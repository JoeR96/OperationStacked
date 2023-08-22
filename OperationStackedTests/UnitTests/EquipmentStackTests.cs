using FluentAssertions;
using NUnit.Framework;
using OperationStacked.Extensions.TemplateExtensions;
using OperationStacked.TestLib;
using OperationStacked.TestLib.Builders;

namespace OperationStackedAuth.Tests.UnitTests;

public class EquipmentStackTests
{
    [Test]
    public void GenerateStack_ReturnsCorrectSequence()
    {
        Decimal?[] expected = { 4.5m, 9.0m, 16.0m, 23.0m};
        
        var equipmentStack = new EquipmentStackBuilder()
            .WithDefaultValues()
            .WithIncrementValue(7.0m)
            .WithInitialIncrements(new decimal?[]{4.5m})
            .WithStartWeight(4.5m)
            .WithIncrementCount(2)
            .Build();

        var result = equipmentStack.GenerateStack();

        result.Should().Equal(expected);
    }
}
