using Concise.Steps;
using MoreLinq.Extensions;
using NUnit.Framework;
using OperationStacked.TestLib;
using OperationStackedAuth.Tests;

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

    [StepTest]
    public async Task CreateRealWorkout()
    {
        var exercises = new List<CreateExerciseModel>();


            var dumbbellOhp = new ExerciseModelBuilder()
                .WithName("Overhead Press")
                .WithReps(8,10)
                .WithEquipmentType(OperationStacked.Enums.EquipmentType.Dumbbell)
                .WithStartingWeight(18)
                .WithLiftDay(1)
                .Adapt();

            var smithInclinePress = new ExerciseModelBuilder()
                .WithName("Incline Smith Press")
                .WithReps(6,8)
                .WithEquipmentType(OperationStacked.Enums.EquipmentType.SmithMachine)
                .WithStartingWeight(47.5m)
                .WithLiftOrder(2)
                .WithLiftDay(1) // Setting the day using the loop variable
                .Adapt();

            var chestFly = new ExerciseModelBuilder()
                .WithName("Chest Fly")
                .WithEquipmentType(OperationStacked.Enums.EquipmentType.Machine)
                .WithStartingWeight(41m)
                .WithLiftOrder(3)
                .WithLiftDay(1) // Setting the day using the loop variable
                .Adapt();
            
            var cableTricepCrucifix = new ExerciseModelBuilder()
                .WithName("Chest Fly")
                .WithEquipmentType(OperationStacked.Enums.EquipmentType.Cable)
                .WithStartingWeight(5.7m)
                .WithReps(12,15)
                .WithSets(4)
                .WithLiftOrder(3)
                .WithLiftDay(1) // Setting the day using the loop variable
                .Adapt();

            var cableLateralRaise = new ExerciseModelBuilder()
                .WithName("Cable Lateral Raise")
                .WithEquipmentType(OperationStacked.Enums.EquipmentType.Cable)
                .WithStartingWeight(5.7m)
                .WithReps(8, 12)
                .WithSets(4)
                .WithLiftOrder(5)
                .Adapt();

            var dumbllMachine = new ExerciseModelBuilder()
                .WithName("Dumbell Machine Curls")
                .WithEquipmentType(OperationStacked.Enums.EquipmentType.Machine)
                .WithStartingWeight(32)
                .WithReps(8, 12)
                .WithSets(4)
                .WithLiftOrder(6)
                .Adapt();

            exercises.AddRange(new List<CreateExerciseModel>
            {
                dumbbellOhp,
                smithInclinePress,
                chestFly,
                cableTricepCrucifix,
                cableLateralRaise,
                dumbllMachine
            });



        var work = await WorkoutClient.WorkoutCreationPOSTAsync((new CreateWorkoutRequest()
        {
            ExerciseDaysAndOrders = exercises,
            UserId = UserId
        }));
    
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
            Id = id,
            Reps = reps,
            Sets = reps.Length
        };
    }
}