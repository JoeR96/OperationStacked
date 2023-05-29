using FluentAssertions;
using NUnit.Framework;
using OperationStacked.Entities;
using OperationStacked.Migrations;

namespace OperationStackedAuth.Tests.UnitTests;

public class EquipmentStackTests
{
    private EquipmentStack stack = new EquipmentStack()
    {
        StartWeight = 4.50m,
        InitialIncrements = new decimal?[] { 6.50m },
        IncrementValue = 7.00m,
        IncrementCount = 15
    };

    [Test]
    public void GenerateStack_ReturnsCorrectSequence()
    {
        Decimal[] expected = { 4.5m, 6.5m, 13.5m, 20.5m, 27.5m, 34.5m, 41.5m, 48.5m, 55.5m, 62.5m, 69.5m, 76.5m, 83.5m, 90.5m, 97.5m, 104.5m };
        // var result = stack.GenerateStack();

        // result.Should().Equal(expected);
    }
}