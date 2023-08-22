using MoreLinq.Extensions;
using NUnit.Framework;
using OperationStacked.TestLib;
using OperationStackedAuth.Tests;
using EquipmentType = OperationStacked.Enums.EquipmentType;

namespace OperationStackedTests.Functional;

public class DataSeed
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
    public async Task CreateExercisesForMultipleDays(int totalDays)
    {
        var exercises = new List<CreateExerciseModel>();

        for (int i = 1; i <= totalDays; i++)
        {
            var linearProgressionBarbell = new ExerciseModelBuilder()
                .WithEquipmentType(EquipmentType.Barbell)
                .WithLiftDay(i)
                .Adapt();
        
            var linearProgressionMachine = new ExerciseModelBuilder()
                .WithEquipmentType(EquipmentType.Machine)
                .WithLiftOrder(2)
                .WithLiftDay(i)   // Setting the day using the loop variable
                .Adapt();
        
            var linearProgressionDumbell = new ExerciseModelBuilder()
                .WithEquipmentType(EquipmentType.Dumbbell)
                .WithLiftOrder(3)
                .WithLiftDay(i)   // Setting the day using the loop variable
                .Adapt();

            exercises.AddRange(new List<CreateExerciseModel>
            {
                linearProgressionBarbell,
                linearProgressionMachine,
                linearProgressionDumbell
            });
        }

        var createWorkoutRequest = new CreateWorkoutRequest()
        {
            ExerciseDaysAndOrders = exercises,
            UserId = UserId
        };

        var createdExercise = await WorkoutClient.WorkoutCreationPOSTAsync(createWorkoutRequest);
        var workout = await WorkoutClient.WorkoutCreationGETAsync(UserId, 1, 1,false);
        workout.Exercises.ForEach(e => CompleteExerciseRecursively(14, e);
         

    }

    private int GenerateExerciseOutcome(double failWeight, double passWeight, double progressWeight)
    {
        var random = new Random();
        var value = random.NextDouble();

        if (value < failWeight) return random.Next(1, 7);     // Fail range
        if (value < failWeight + passWeight) return random.Next(8, 12);  // Pass range
        return 12;  // Progress
    }

    public async Task CompleteExerciseRecursively(int count, LinearProgressionExercise exercise, 
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
        }
    }

    
    private CompleteExerciseRequest CreateExerciseRequest(int[] reps,Guid id)
    {
        return new CompleteExerciseRequest
        {
            Id = id,
            Reps = reps,
            Sets = reps.Length
        };
    }
}