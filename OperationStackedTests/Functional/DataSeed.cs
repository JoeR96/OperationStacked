using Concise.Steps;
using MoreLinq.Extensions;
using NUnit.Framework;
using OperationStacked.TestLib;
using OperationStackedAuth.Tests;

namespace OperationStackedTests.Functional;
[TestFixture]
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

    [StepTest]
    public async Task CreateExercisesForMultipleDays()
    {
        var exercises = new List<CreateExerciseModel>();

        for (int i = 1; i <= 5; i++)
        {
            string exerciseNameBarbell = "";
            string exerciseNameMachine = "";
            string exerciseNameDumbbell = "";

            switch (i)
            {
                case 1:
                    exerciseNameBarbell = "Chest Press (Barbell)";
                    exerciseNameMachine = "Chest Fly (Machine)";
                    exerciseNameDumbbell = "Chest Press (Dumbbell)";
                    break;
                case 2:
                    exerciseNameBarbell = "Squat (Barbell)";
                    exerciseNameMachine = "Leg Press (Machine)";
                    exerciseNameDumbbell = "Lunges (Dumbbell)";
                    break;
                case 3:
                    exerciseNameBarbell = "Deadlift (Barbell)";
                    exerciseNameMachine = "Lat Pull Down (Machine)";
                    exerciseNameDumbbell = "Bent Over Row (Dumbbell)";
                    break;
                case 4:
                    exerciseNameBarbell = "Functional Lift (Barbell)";
                    exerciseNameMachine = "Functional Pull (Machine)";
                    exerciseNameDumbbell = "Functional Press (Dumbbell)";
                    break;
                case 5:
                    exerciseNameBarbell = "Functional Lift (Barbell)";
                    exerciseNameMachine = "Functional Pull (Machine)";
                    exerciseNameDumbbell = "Functional Press (Dumbbell)";
                    break;
                default:
                    // For days beyond 4, you can add default exercises or extend the switch-case.
                    break;
            }

            var linearProgressionBarbell = new ExerciseModelBuilder()
                .WithDefaultValues()
                .WithName(exerciseNameBarbell)
                .WithEquipmentType(OperationStacked.Enums.EquipmentType.Barbell)
                .WithLiftDay(i)
                .Adapt();

            var linearProgressionMachine = new ExerciseModelBuilder()
                .WithDefaultValues()
                .WithName(exerciseNameMachine)
                .WithEquipmentType(OperationStacked.Enums.EquipmentType.Machine)
                .WithLiftOrder(2)
                .WithLiftDay(i) // Setting the day using the loop variable
                .Adapt();

            var linearProgressionDumbell = new ExerciseModelBuilder()
                .WithDefaultValues()
                .WithName(exerciseNameDumbbell)
                .WithEquipmentType(OperationStacked.Enums.EquipmentType.Dumbbell)
                .WithLiftOrder(3)
                .WithLiftDay(i) // Setting the day using the loop variable
                .Adapt();

            exercises.AddRange(new List<CreateExerciseModel>
            {
                linearProgressionBarbell,
                linearProgressionMachine,
                linearProgressionDumbell
            });


        }

        var work = await WorkoutClient.WorkoutCreationPOSTAsync((new CreateWorkoutRequest()
        {
            ExerciseDaysAndOrders = exercises,
            UserId = UserId
        }));
        var tasks = new List<Task>();

        foreach (var workExercise in work.Exercises)
        {
            tasks.Add(CompleteExerciseRecursivelyAsync(20, workExercise));
        }

        await Task.WhenAll(tasks);
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