using FluentAssertions;
using NUnit.Framework;
using OperationStacked.Extensions.TemplateExtensions;
using OperationStacked.TestLib;
using OperationStacked.TestLib.Adapters;
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
        // Create exercise builders with specific details
        var exerciseBuilders = new List<ExerciseBuilder>
        {
            new ExerciseBuilder().WithDefaultValues().WithExerciseName("Barbell Squat").WithCategory((OperationStacked.Enums.Category)Category._5).WithEquipmentType(EquipmentType.Barbell).WithUserId(_userId),
            new ExerciseBuilder().WithDefaultValues().WithExerciseName("Leg Press").WithCategory((OperationStacked.Enums.Category)Category._5).WithEquipmentType(EquipmentType.Machine).WithUserId(_userId),
            new ExerciseBuilder().WithDefaultValues().WithExerciseName("Dumbbell Lunges").WithCategory((OperationStacked.Enums.Category)Category._5).WithEquipmentType(EquipmentType.Dumbbell).WithUserId(_userId)
        };

        // Create workout exercise builders with lift day and order
        var workoutExerciseBuilders = new List<WorkoutExerciseBuilder>
        {
            new WorkoutExerciseBuilder().WithDefaultValues().WithLiftDay(1).WithLiftOrder(1),
            new WorkoutExerciseBuilder().WithDefaultValues().WithLiftDay(1).WithLiftOrder(2),
            new WorkoutExerciseBuilder().WithDefaultValues().WithLiftDay(1).WithLiftOrder(3)
        };

        // Build the exercises
        var exercises = exerciseBuilders.Zip(workoutExerciseBuilders, (exBuilder, wExBuilder) =>
        {
            var exercise = exBuilder.Build();
            var workoutExercise = wExBuilder.WithExerciseId(exercise.Id).Build();
            return  new LinearProgressionExerciseBuilder().WithDefaultValues().WithWorkingWeight(100).AdaptToCreateRequest(workoutExercise.AdaptToCreateRequest());
        }).ToList();

        var createWorkoutRequest = new CreateWorkoutRequest
        {
            UserId = _userId,
            Exercises = exercises
        };

        var createdWorkout = await _workoutClient.WorkoutCreationPOSTAsync(createWorkoutRequest);
        createdWorkout.Should().NotBeNull();

        var completeBarbellResponse = await _workoutClient.CompleteAsync(new CompleteExerciseRequest()
        {
            ExerciseId = createdWorkout.Exercises.ToList()[0].Id,
            Reps = new int[]{12,12,12},
            Sets = 3
        });

        var barbellProgression = createdWorkout.Exercises.ToList()[1].WorkingWeight;
        var barbellProgressWeight = barbellProgression + createdWorkout.Exercises.ToList()[2].WeightProgression;
        completeBarbellResponse.Exercise.WorkingWeight.Should().Be(barbellProgressWeight);
        
        var completeMachineResponse = await _workoutClient.CompleteAsync(new CompleteExerciseRequest()
        {
            ExerciseId = createdWorkout.Exercises.ToList()[1].Id,
            Reps = new int[]{12,12,12},
            Sets = 3
        });

        var machineProgressWeight = new EquipmentStackBuilder().WithDefaultValues().Build().GenerateStack();
        var weight = machineProgressWeight[completeMachineResponse.Exercise.WeightIndex];
        
        completeMachineResponse.Exercise.WorkingWeight.Should().Be(weight);
        
        var completeDumbBellResponse = await _workoutClient.CompleteAsync(new CompleteExerciseRequest()
        {
            ExerciseId = createdWorkout.Exercises.ToList()[2].Id,
            Reps = new int[]{12,12,12},
            Sets = 3
        });
        
        //in our context dumbells go up in 1's up until 10 then 12,14,16,18 etc.. in 2's
        var dumbbellProgression =createdWorkout.Exercises.ToList()[2].WorkingWeight;
        var dumbBellProgressWeight = dumbbellProgression + createdWorkout.Exercises.ToList()[2].WeightProgression;
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
