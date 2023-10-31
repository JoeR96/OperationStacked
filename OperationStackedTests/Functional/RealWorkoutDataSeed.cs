using Concise.Steps;
using MoreLinq.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;
using OperationStacked.Data;
using OperationStacked.Requests;
using OperationStacked.TestLib;
using OperationStacked.TestLib.Adapters;
using OperationStacked.TestLib.Builders;
using OperationStackedAuth.Tests;
using CompleteExerciseRequest = OperationStacked.TestLib.CompleteExerciseRequest;
using CreateEquipmentStackRequest = OperationStacked.TestLib.CreateEquipmentStackRequest;
using CreateLinearProgressionExerciseRequest = OperationStacked.TestLib.CreateLinearProgressionExerciseRequest;
using CreateWorkoutRequest = OperationStacked.TestLib.CreateWorkoutRequest;

namespace OperationStackedTests.Functional;
[TestFixture]
public class RealWorkoutDataSeed
{
    public Guid UserId;
    public WorkoutClient WorkoutClient;
    public List<Exercise> Workout;

    [OneTimeSetUp]
    public async Task InitiateClient()
    {
        var (client, _userId) = await ApiClientFactory.CreateWorkoutClientAsync(true);
        WorkoutClient = client;
        UserId = _userId;
    }
     [Test]
    public async Task GenerateFiveDaySplit()
    {
        var fiveDayWorkout = new List<CreateLinearProgressionExerciseRequest>();
        var userId = UserId;
        // Assuming different weight progression for compound (2.5kg) and isolation (1kg) exercises
        decimal compoundProgression = 2.5m;
        decimal isolationProgression = 1.0m;

        // Day 1: Chest & Triceps
// Day 1: Chest & Triceps
fiveDayWorkout.AddRange(new List<CreateLinearProgressionExerciseRequest>
{
    CreateExerciseRequest("Bench Press", Category._1, EquipmentType._0, 5, 5, 5, 60, 2.5m, 1, 1, userId),
    CreateExerciseRequest("Incline Dumbbell Press", Category._1, EquipmentType._2, 8, 10, 3, 22.5m, 2.5m, 1, 2, userId),
    CreateExerciseRequest("Chest Fly Machine", Category._1, EquipmentType._3, 10, 12, 3, 40, 1.0m, 1, 3, userId),
    CreateExerciseRequest("Tricep Dips", Category._4, EquipmentType._1, 6, 10, 4, 0, 0m, 1, 4, userId),
    CreateExerciseRequest("Tricep Overhead Extension", Category._4, EquipmentType._2, 8, 12, 3, 14, 1.0m, 1, 5, userId)
});

// Day 2: Back & Biceps
fiveDayWorkout.AddRange(new List<CreateLinearProgressionExerciseRequest>
{
    CreateExerciseRequest("Pull-Ups", Category._2, EquipmentType._1, 6, 8, 4, 0, 0m, 2, 1, userId),
    CreateExerciseRequest("Barbell Rows", Category._2, EquipmentType._0, 5, 7, 5, 50, 2.5m, 2, 2, userId),
    CreateExerciseRequest("Lat Pull Down", Category._2, EquipmentType._4, 8, 10, 3, 45, 1.0m, 2, 3, userId),
    CreateExerciseRequest("Seated Cable Row", Category._2, EquipmentType._4, 10, 12, 3, 40, 1.0m, 2, 4, userId),
    CreateExerciseRequest("Barbell Curl", Category._3, EquipmentType._0, 8, 10, 3, 30, 1.0m, 2, 5, userId)
});

// Day 3: Legs
fiveDayWorkout.AddRange(new List<CreateLinearProgressionExerciseRequest>
{
    CreateExerciseRequest("Squat", Category._5, EquipmentType._0, 5, 5, 5, 70, 2.5m, 3, 1, userId),
    CreateExerciseRequest("Deadlift", Category._5, EquipmentType._0, 5, 5, 4, 100, 2.5m, 3, 2, userId),
    CreateExerciseRequest("Leg Press", Category._5, EquipmentType._3, 8, 12, 3, 120, 5.0m, 3, 3, userId),
    CreateExerciseRequest("Lunges", Category._5, EquipmentType._2, 10, 12, 3, 20, 1.0m, 3, 4, userId),
    CreateExerciseRequest("Calf Raises", Category._5, EquipmentType._3, 12, 15, 4, 60, 1.0m, 3, 5, userId)
});

// Day 4: Shoulders & Abs
fiveDayWorkout.AddRange(new List<CreateLinearProgressionExerciseRequest>
{
    CreateExerciseRequest("Overhead Press", Category._0, EquipmentType._0, 5, 7, 5, 40, 2.5m, 4, 1, userId),
    CreateExerciseRequest("Lateral Raise", Category._0, EquipmentType._2, 10, 15, 3, 8, 0.5m, 4, 2, userId),
    CreateExerciseRequest("Front Raise", Category._0, EquipmentType._2, 10, 12, 3, 8, 0.5m, 4, 3, userId),
    CreateExerciseRequest("Shrugs", Category._0, EquipmentType._0, 8, 10, 3, 60, 2.5m, 4, 4, userId),
    CreateExerciseRequest("Cable Reverse Fly", Category._0, EquipmentType._4, 12, 15, 3, 10, 1.0m, 4, 5, userId)
});

// Day 5: Full Body
fiveDayWorkout.AddRange(new List<CreateLinearProgressionExerciseRequest>
{
    // Assuming FullBody is _4, but you haven't provided a mapping for FullBody in your original enums.
    CreateExerciseRequest("Clean and Press", Category._4, EquipmentType._0, 6, 8, 4, 50, 2.5m, 5, 1, userId),
    CreateExerciseRequest("Smith Machine Squat", Category._5, EquipmentType._1, 8, 10, 3, 60, 2.5m, 5, 2, userId),
    CreateExerciseRequest("Incline Bench Press", Category._1, EquipmentType._1, 8, 10, 3, 55, 2.5m, 5, 3, userId),
    CreateExerciseRequest("One-Arm Dumbbell Row", Category._2, EquipmentType._2, 8, 10, 3, 25, 1.0m, 5, 4, userId),
    CreateExerciseRequest("Dumbbell Hammer Curls", Category._3, EquipmentType._2, 10, 12, 3, 14, 1.0m, 5, 5, userId)
});

var createWorkoutRequest = new CreateWorkoutRequest()
{
    Exercises = fiveDayWorkout,
    UserId = UserId,
    WorkoutName = "Joe's BigDaddyWorkout"
};

// Serialize the request to JSON
var workoutRequestJson = JsonConvert.SerializeObject(createWorkoutRequest, Formatting.Indented);

Console.WriteLine(workoutRequestJson);

await WorkoutClient.WorkoutCreationPOSTAsync(new CreateWorkoutRequest()
{
    Exercises = fiveDayWorkout,
    UserId = userId,
    WorkoutName = "Joe's BigDaddyWorkout"
});
    }

public CreateLinearProgressionExerciseRequest CreateExerciseRequest(
    string name,
    Category category,
    EquipmentType equipmentType,
    int minReps,
    int maxReps,
    int sets,
    decimal startingWeight,
    decimal weightProgression,
    int liftDay,
    int liftOrder,
    Guid userId)
{
    // Initialize builders
    var exerciseBuilder = new ExerciseBuilder()
        .WithExerciseName(name)
        .WithCategory((OperationStacked.Enums.Category)category)
        .WithEquipmentType((OperationStacked.Enums.EquipmentType)equipmentType)
        .WithUserId(userId);

    var linearProgressionBuilder = new LinearProgressionExerciseBuilder()
        .WithDefaultValues()
        .WithReps(minReps, maxReps)
        .WithSets(sets)
        .WithWeightProgression(weightProgression);

    var workoutExerciseBuilder = new WorkoutExerciseBuilder()
        .WithDefaultValues()
        .WithLiftDay(liftDay)
        .WithLiftOrder(liftOrder);

    CreateEquipmentStackRequest equipmentStackRequest = null;

    // Check for specific equipment types that require a stack
    if (equipmentType == EquipmentType._3 || equipmentType == EquipmentType._4)
    {
        // Use the EquipmentStackBuilder to build the CreateEquipmentStackRequest
        var equipmentStackBuilder = new EquipmentStackBuilder()
            .WithDefaultValues()
            .WithStartWeight(startingWeight)
            // Add additional configurations as necessary
            .WithIncrementValue(weightProgression)
            .WithUserId(userId);

        equipmentStackRequest = equipmentStackBuilder.BuildCreateRequest();
    }

    // Build and return the request
    var createLinearProgressionExerciseRequest = new CreateLinearProgressionExerciseRequest
    {
        MinimumReps = minReps,
        MaximumReps = maxReps,
        TargetSets = sets,
        WeightIndex = 0, // Assuming this starts at 0 for a new workout plan
        WeightProgression = weightProgression,
        AttemptsBeforeDeload = 3, // Adjust as needed
        EquipmentType = equipmentType,
        WorkoutExercise = new OperationStacked.TestLib.CreateWorkoutExerciseRequest
        {
            LiftDay = liftDay,
            LiftOrder = liftOrder,
            Exercise = exerciseBuilder.BuildCreateExerciseRequest(),
            ExerciseId = Guid.NewGuid(), // Assuming a new ID is generated for each new exercise
            RestTimer = 90 // Assuming 90 seconds rest between sets
        },
        EquipmentStack = equipmentStackRequest // Assign the built equipment stack request here
    };

    return createLinearProgressionExerciseRequest;
}


    private int GenerateExerciseOutcome(double failWeight, double passWeight, double progressWeight)
    {
        var random = new Random();
        var value = random.NextDouble();

        if (value < failWeight) return random.Next(1, 7);     // Fail range
        if (value < failWeight + passWeight) return random.Next(8, 12);  // Pass range
        return 12;  // Progress
    }

    public async Task CompleteExerciseRecursivelyAsync(int count, LinearProgressionExercise exercise, 
        double failWeight = 0.05, 
        double passWeight = 0.4, 
        double progressWeight = 0.4)
    {
        var exerciseId = exercise.Id;

        for (int i = 0; i < count; i++)
        {
            var outcome = GenerateExerciseOutcome(failWeight, passWeight, progressWeight);
            var _ = CreateExerciseRequest(new int[] { outcome, outcome, outcome }, exerciseId);

            var completeResponse = await WorkoutClient.CompleteAsync(_);
            exerciseId = completeResponse.Exercise.Id;
            Console.WriteLine(completeResponse.Exercise.Id);
        }
    }

    
    private CompleteExerciseRequest CreateExerciseRequest(int[] reps,Guid id)
    {
        return new CompleteExerciseRequest
        {
            ExerciseId = id,
            Reps = reps,
            Sets = reps.Length
        };
    }
}
