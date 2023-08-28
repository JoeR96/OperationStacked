using FluentAssertions;
using MySqlX.XDevAPI;
using NUnit.Framework;
using OperationStacked.Extensions.TemplateExtensions;
using OperationStacked.TestLib;
using OperationStacked.TestLib.Builders;
using EquipmentType = OperationStacked.Enums.EquipmentType;

namespace OperationStackedAuth.Tests.UnitTests.RecipeTests;

[TestFixture]
public class LinearProgressionWorkoutTests
{
    private WorkoutClient _workoutClient { get; set; }
    private Guid _userId { get; set; }
    [OneTimeSetUp]
    public async Task InitiateClient()
    {
        var (client, userId) = await ApiClientFactory.CreateWorkoutClientAsync(true);
        _workoutClient = client;
        _userId = userId;
    }


    [Test]
    public async Task LinearProgressionExercise_CreatesWithValues()
    {
        var linearProgressionBarbell = new ExerciseModelBuilder()
            .WithEquipmentType(EquipmentType.Barbell)
            .Adapt();
        
        var linearProgressionMachine = new ExerciseModelBuilder()
            .WithEquipmentType(EquipmentType.Machine)
            .WithLiftOrder(2)
            .Adapt();
        
        var linearProgressionDumbell = new ExerciseModelBuilder()
            .WithEquipmentType(EquipmentType.Dumbbell)
            .WithLiftOrder(3)
            .Adapt();

        var exercises = new List<CreateExerciseModel>();

        exercises.Add(linearProgressionBarbell);
        exercises.Add(linearProgressionMachine);
        exercises.Add(linearProgressionDumbell);

        var CreateWorkoutRequest = new CreateWorkoutRequest()
        {
            ExerciseDaysAndOrders = exercises,
            UserId = _userId
        };
        var createdExercise = await _workoutClient.WorkoutCreationPOSTAsync(CreateWorkoutRequest);
        var workout = await _workoutClient.WorkoutCreationGETAsync(_userId, 1, 1, false);

        var completeBarbellResponse = await _workoutClient.CompleteAsync(new CompleteExerciseRequest()
        {
            Id = workout.Exercises.ToList()[0].Id,
            Reps = new int[]{12,12,12},
            Sets = 3
        });

        var barbellProgressWeight = linearProgressionBarbell.StartingWeight +=
            linearProgressionBarbell.WeightProgression;
        completeBarbellResponse.Exercise.WorkingWeight.Should().Be(barbellProgressWeight);
        
        var completeMachineResponse = await _workoutClient.CompleteAsync(new CompleteExerciseRequest()
        {
            Id = workout.Exercises.ToList()[1].Id,
            Reps = new int[]{12,12,12},
            Sets = 3
        });

        var machineProgressWeight = new EquipmentStackBuilder().WithDefaultValues().Build().GenerateStack();
        var weight = (double)machineProgressWeight[completeMachineResponse.Exercise.WeightIndex];
        
        completeMachineResponse.Exercise.WorkingWeight.Should().Be(weight);
        
        var completeDumbBellResponse = await _workoutClient.CompleteAsync(new CompleteExerciseRequest()
        {
            Id = workout.Exercises.ToList()[2].Id,
            Reps = new int[]{12,12,12},
            Sets = 3
        });
        
        //in our context dumbells go up in 1's up until 10 then 12,14,16,18 etc.. in 2's
        var dumbBellProgressWeight = linearProgressionDumbell.StartingWeight + 2;
        completeDumbBellResponse.Exercise.WorkingWeight.Should().Be(dumbBellProgressWeight);
        completeDumbBellResponse.Exercise.LiftWeek.Should().Be(2);
    }
    
    [OneTimeTearDown]
    public async Task TearDown()
    {
        // var delete = await _workoutClient.DeleteAllAsync(_userId);
        // delete.Should().Be(true);
    }
}