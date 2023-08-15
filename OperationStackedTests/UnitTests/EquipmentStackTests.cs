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

public static class EquipmentStackExtensions
{
    public static Decimal?[] GenerateStack(this EquipmentStack e)
    {
            
        List<Decimal?> stack = new List<Decimal?>();
        stack.Add(e.StartWeight);

        foreach (var increment in e.InitialIncrements)
        {
            var t = stack.Last();
            stack.Add(t += increment);
        }

        for (int i = 0; i < e.IncrementCount; i++)
        {
            var t = stack.Last();
            stack.Add(t += e.IncrementValue);
        }

        return stack.ToArray();
    }
}