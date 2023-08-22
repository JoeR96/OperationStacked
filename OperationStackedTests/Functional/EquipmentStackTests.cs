using Concise.Steps;
using FluentAssertions;
using NUnit.Framework;
using OperationStacked.TestLib;
using OperationStacked.TestLib.Builders;
using OperationStackedAuth.Tests;

namespace OperationStackedTests.Functional;

[TestFixture]
[NonParallelizable]
public class EquipmentStackTests
{
    public WorkoutClient workoutClient;
    public Guid userId;

    [OneTimeSetUp]
    public async Task InitiateClient()
    {
        var (client, _userId) = await ApiClientFactory.CreateWorkoutClientAsync(true);
        workoutClient = client;
        userId = _userId;
    }
    [StepTest]
    public async Task EquipmentStack_CreatesWithCorrectStartWeightAndIncrement()
    {
        await "Create the equipment stack".__(async () =>
        {

            var request = new EquipmentStackBuilder().WithDefaultValues().Adapt();
            var response = await workoutClient.CreateAsync(request);

            var stack = await workoutClient.EquipmentStackGETAsync(response.Stack.Id);
            
            stack.Stack.EquipmentStackKey.Should().Be(request.EquipmentStackKey);
            
            
        });
    }
}