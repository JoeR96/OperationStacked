using Concise.Steps;
using FluentAssertions;
using NUnit.Framework;
using OperationStacked.TestLib;
using OperationStacked.TestLib.Adapters;
using OperationStackedAuth.Tests;
using Exercise = OperationStacked.TestLib.Exercise;

namespace OperationStackedTests.Functional
{
    [TestFixture]
    [NonParallelizable]
    public class LinearProgressionProgressTests
    {
        public Guid userId;
        public WorkoutClient workoutClient;
        public List<WorkoutExercise> Workout;

        [OneTimeSetUp]
        public async Task InitiateClient()
        {
            var (client, _userId) = await ApiClientFactory.CreateWorkoutClientAsync(true);
            workoutClient = client;
            userId = _userId;
        }

        [StepTest, Order(0)]
        public async Task EquipmentStack_CreatesWithMultipleStartWeights()
        {
            var request = new CreateEquipmentStackRequest()
            {
                StartWeight = 1.1m,
                InitialIncrements = new decimal[] { 1.1m },
                IncrementValue = 2.3m,
                IncrementCount = 20,
                EquipmentStackKey = "The Gym Cable"
            };

            await "The Equipment Stack is created".__(async () =>
            {
                var response = await workoutClient.CreateAsync(request);
            });
        }

[StepTest, Order(10)]
public async Task LinearProgression_WorkoutCreatesWithAllEquipmentTypes()
{
    var exercises = new List<CreateLinearProgressionExerciseRequest>
    {
        BuildLinearProgressionExerciseRequest("Bench Press", Category._1, EquipmentType._0, 1, 1, 60.00m, 2.50m, 2, true, 1, 8, 12, 3, 0, EquipmentStackKey._0, userId),
        BuildLinearProgressionExerciseRequest("Barbell Row", Category._2, EquipmentType._0, 1, 2, 60, 2.5m, 0, false, 1, 8, 12, 3, 0, EquipmentStackKey._0, userId),
        BuildLinearProgressionExerciseRequest("Incline Smith Press", Category._0, EquipmentType._1, 1, 3, 30.00m, 2.5m, 2, false, 1, 8, 10, 3, 0, EquipmentStackKey._1, userId),
        BuildLinearProgressionExerciseRequest("Hammer Curl", Category._3, EquipmentType._2, 1, 4, 12.00m, 1.0m, 2, false, 1, 8, 12, 3, 0, EquipmentStackKey._2, userId),
        BuildLinearProgressionExerciseRequest("Tricep PullDown", Category._4, EquipmentType._4, 1, 5, 20, 1.0m, 2, false, 1, 12, 15, 3, 5, EquipmentStackKey._2, userId),
        BuildLinearProgressionExerciseRequest("Lateral Raise", Category._0, EquipmentType._2, 1, 6, 6.00m, 1.0m, 2, false, 1, 8, 12, 4, 7, EquipmentStackKey._2, userId),
        BuildLinearProgressionExerciseRequest("Back Squat", Category._5, EquipmentType._0, 2, 1, 60.00m, 5.0m, 2, true, 1, 8, 12, 3, 0, EquipmentStackKey._0, userId),
        BuildLinearProgressionExerciseRequest("Romanian Deadlift", Category._5, EquipmentType._0, 2, 2, 80, 5.0m, 0, false, 1, 8, 12, 3, 0, EquipmentStackKey._0, userId),
        BuildLinearProgressionExerciseRequest("Leg Extension", Category._5, EquipmentType._3, 2, 3, 4.5m, 4.3m, 2, false, 1, 8, 12, 3, 0, EquipmentStackKey._2, userId),
        BuildLinearProgressionExerciseRequest("Hammer Curl", Category._3, EquipmentType._2, 2, 4, 12.00m, 1.0m, 2, false, 1, 8, 12, 3, 0, EquipmentStackKey._2, userId),
        BuildLinearProgressionExerciseRequest("Unilateral Pull Down", Category._2, EquipmentType._4, 3, 1, 20, 1.0m, 2, false, 1, 8, 12, 3, 9, EquipmentStackKey._2, userId),
        BuildLinearProgressionExerciseRequest("Overhead Press", Category._1, EquipmentType._0, 3, 2, 35, 2.5m, 2, false, 1, 8, 12, 3, 0, EquipmentStackKey._0, userId),
        BuildLinearProgressionExerciseRequest("Unilateral Pull Down", Category._2, EquipmentType._4, 3, 3, 20, 1.0m, 2, false, 1, 8, 12, 3, 1, EquipmentStackKey._2, userId),
        BuildLinearProgressionExerciseRequest("Lateral Raise", Category._1, EquipmentType._2, 3, 4, 6.00m, 1.0m, 2, false, 1, 8, 12, 3, 0, EquipmentStackKey._2, userId),
        BuildLinearProgressionExerciseRequest("Bicep Curl", Category._3, EquipmentType._2, 3, 5, 12.00m, 1.0m, 2, false, 1, 8, 12, 3, 0, EquipmentStackKey._2, userId),
        BuildLinearProgressionExerciseRequest("Unilateral Tricep Pull Down", Category._4, EquipmentType._4, 3, 6, 15, 1.0m, 2, false, 1, 12, 15, 3, 2, EquipmentStackKey._2, userId),
        BuildLinearProgressionExerciseRequest("Bulgarian Split Squat", Category._5, EquipmentType._2, 4, 1, 10, 1.0m, 0, false, 1, 8, 12, 3, 0, EquipmentStackKey._2, userId),
        BuildLinearProgressionExerciseRequest("Leg Press", Category._5, EquipmentType._0, 4, 2, 80, 10.0m, 0, false, 1, 8, 12, 3, 0, EquipmentStackKey._0, userId),
        BuildLinearProgressionExerciseRequest("Leg Curl", Category._5, EquipmentType._3, 4, 3, 4.5m, 4.3m, 2, false, 1, 8, 12, 3, 0, EquipmentStackKey._2, userId),
        BuildLinearProgressionExerciseRequest("Bicep Curl", Category._4, EquipmentType._2, 4, 4, 12.00m, 1.0m, 2, false, 1, 8, 12, 3, 0, EquipmentStackKey._2, userId),
        // Add any additional exercises you have in the same pattern
    };

    var request = new CreateWorkoutRequest
    {
        UserId = userId,
        Exercises = exercises
    };

    WorkoutCreationResult workoutCreationResult;
    await "The workout is created and returned.".__(async () =>
    {
        workoutCreationResult = await workoutClient.WorkoutCreationPOSTAsync(request);
        workoutCreationResult.Exercises.Count.Should().Be(exercises.Count);
    });
}

        [StepTest, Order(20)]
        public async Task LinearProgression_WorkoutIsRetrieved()
        {
            await "The workout is retrieved".__(async () =>
            {
                var getWorkoutResponse = await workoutClient.WorkoutCreationGETAsync(userId, 1, 1, false);
                Workout = getWorkoutResponse.Exercises.ToList();

                foreach (var exercise in Workout)
                {
                    exercise.LinearProgressionExercise.LiftWeek.Should().Be(1);
                }

                Workout.Count().Should().Be(6);

                Workout.ForEach(e => e.LiftDay.Should().Be(1));
                Workout.ForEach(e => e.LinearProgressionExercise.LiftWeek.Should().Be(1));
                Workout.ForEach(e => e.LinearProgressionExercise.WorkingWeight.Should().BeGreaterThan(0));

            });
        }

        [StepTest, Order(30)]
        public async Task LinearProgression_BarbellExerciseProgresses()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    ExerciseId = Workout[0].Id,
                    Reps = new int[] { 12, 12, 12 },
                    Sets = 3
                });

                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(15);
                completionResponse.Exercise.ParentId.Should().Be(Workout[0].Id);
            });
        }

        [StepTest, Order(40)]
        public async Task LinearProgression_SmithMachineExerciseProgresses()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    ExerciseId = Workout[1].Id,
                    Reps = new int[] { 12, 12, 12 },
                    Sets = 3
                });

                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(12.5m);
                completionResponse.Exercise.ParentId.Should().Be(Workout[1].Id);
            });
        }

        [StepTest, Order(50)]
        public async Task LinearProgression_DumbbellProgressesBy1KGWhenWorkingWeightIsBelow9KG()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    ExerciseId = Workout[2].Id,
                    Reps = new int[] { 12, 12, 12 },
                    Sets = 3
                });

                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(9);
                completionResponse.Exercise.ParentId.Should().Be(Workout[2].Id);
            });
        }

        [StepTest, Order(60)]
        public async Task LinearProgression_DumbbellProgressesBy2KGWhenWorkingWeightIsAbove9KG()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    ExerciseId = Workout[3].Id,
                    Reps = new int[] { 12, 12, 12 },
                    Sets = 3
                });

                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(12);
                completionResponse.Exercise.ParentId.Should().Be(Workout[3].Id);
            });
        }

        [StepTest, Order(70)]
        public async Task LinearProgression_MachineIndexProgress()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    ExerciseId = Workout[4].Id,
                    Reps = new int[] { 12, 12, 12 },
                    Sets = 3
                });

                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(11);
                completionResponse.Exercise.ParentId.Should().Be(Workout[4].Id);

            });
        }

        [StepTest, Order(80)]
        public async Task LinearProgression_CableIndexProgress()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    ExerciseId = Workout[5].Id,
                    Reps = new int[] { 12, 12, 12 },
                    Sets = 3
                });

                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(5.70m);
                completionResponse.Exercise.ParentId.Should().Be(Workout[5].Id);
            });
        }

        [StepTest, Order(90)]
        public async Task LinearProgression_CurrentWeekValuesShouldNotChange()
        {
            await "retrieve current workout after all exercises have completed and saved to the Database.".__(
                async () =>
                {
                    var getWorkoutResponse = await workoutClient.WorkoutCreationGETAsync(userId, 1, 1, true);
                    var retrievedWorkout = getWorkoutResponse.Exercises.ToList();

                    retrievedWorkout.Count().Should().Be(Workout.Count);
                });
        }

        [StepTest, Order(100)]
        public async Task LinearProgression_CurrenWorkoutCompletesAndIncreaseWeekCount()
        {
            await "Complete the workout.".__(
                async () =>
                {
                    var completeWorkoutResponse = await workoutClient.UpdateWeekAndDayAsync(
                        new UpdateWeekAndDayRequest()
                        {

                        });

                    completeWorkoutResponse.Week.Should().Be(1);
                    completeWorkoutResponse.Day.Should().Be(2);
                });
        }

        private List<WorkoutExercise> WorkoutTwo;

        [StepTest, Order(110)]
        public async Task LinearProgression_WorkoutTwoIsRetrieved()
        {
            await "The workout is retrieved".__(async () =>
            {
                var getWorkoutResponse = await workoutClient.WorkoutCreationGETAsync(userId, 1, 2, false);
                WorkoutTwo = getWorkoutResponse.Exercises.ToList();

                foreach (var exercise in Workout)
                {
                    exercise.LinearProgressionExercise.LiftWeek.Should().Be(1);
                }

                WorkoutTwo.Count().Should().Be(6);

                WorkoutTwo.ForEach(e => e.LinearProgressionExercise.LiftWeek.Should().Be(1));
                WorkoutTwo.ForEach(e => e.LiftDay.Should().Be(2));

            });
        }

        [StepTest, Order(120)]
        public async Task LinearProgression_BarbellExerciseUnderMaxAboveMinimumStaysTheSame()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    ExerciseId = WorkoutTwo[0].Id,
                    Reps = new int[] { 8, 8, 8 },
                    Sets = 3
                });

                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should()
                    .Be(WorkoutTwo[0].LinearProgressionExercise.WorkingWeight);
                completionResponse.Exercise.ParentId.Should().Be(WorkoutTwo[0].Id);
            });
        }

        [StepTest, Order(130)]
        public async Task LinearProgression_SmithMachineExerciseUnderMaxAboveMinimumStaysTheSame()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    ExerciseId = WorkoutTwo[1].Id,
                    Reps = new int[] { 11, 11, 11 },
                    Sets = 3
                });

                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should()
                    .Be(WorkoutTwo[1].LinearProgressionExercise.WorkingWeight);
                completionResponse.Exercise.ParentId.Should().Be(WorkoutTwo[1].Id);
            });
        }

        [StepTest, Order(140)]
        public async Task LinearProgression_DumbbellExerciseUnderMaxAboveMinimumStaysTheSameWhenBelow9KG()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    ExerciseId = WorkoutTwo[2].Id,
                    Reps = new int[] { 11, 11, 11 },
                    Sets = 3
                });

                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should()
                    .Be(WorkoutTwo[2].LinearProgressionExercise.WorkingWeight);
                completionResponse.Exercise.ParentId.Should().Be(WorkoutTwo[2].Id);
            });
        }

        [StepTest, Order(150)]
        public async Task LinearProgression_DumbbellExerciseUnderMaxAboveMinimumStaysTheSameWhenAbove9KG()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    ExerciseId = WorkoutTwo[3].Id,
                    Reps = new int[] { 11, 11, 11 },
                    Sets = 3
                });

                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkoutExercise.LinearProgressionExercise.WorkingWeight.Should()
                    .Be(WorkoutTwo[3].LinearProgressionExercise.WorkingWeight);
                completionResponse.Exercise.ParentId.Should().Be(WorkoutTwo[3].Id);
            });
        }

        [StepTest, Order(160)]
        public async Task LinearProgression_MachineIndexUnderMaxAboveMinimumStaysTheSame()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    ExerciseId = WorkoutTwo[4].Id,
                    Reps = new int[] { 11, 11, 11 },
                    Sets = 3
                });

                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should()
                    .Be(WorkoutTwo[4].LinearProgressionExercise.WorkingWeight);
                completionResponse.Exercise.ParentId.Should().Be(WorkoutTwo[4].Id);

            });
        }

        [StepTest, Order(170)]
        public async Task LinearProgression_CableIndexExerciseUnderMaxAboveMinimumStaysTheSame()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    Reps = new int[] { 11, 11, 11 },
                    Sets = 3
                });

                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should()
                    .Be(WorkoutTwo[5].LinearProgressionExercise.WorkingWeight);
                completionResponse.Exercise.ParentId.Should().Be(WorkoutTwo[5].Id);
            });
        }

        [StepTest, Order(180)]
        public async Task LinearProgression_CurrentWeekValuesShouldNotChangeForDayTwo()
        {
            await "retrieve current workout after all exercises have completed and saved to the Database.".__(
                async () =>
                {
                    var getWorkoutResponse = await workoutClient.WorkoutCreationGETAsync(userId, 1, 1, true);
                    var retrievedWorkout = getWorkoutResponse.Exercises.ToList();

                    retrievedWorkout.Count().Should().Be(WorkoutTwo.Count);
                });
        }

        [StepTest, Order(190)]
        public async Task LinearProgression_CurrenWorkoutCompletesAndIncreaseWeekCountAndDayResetsToOne()
        {
            await "Complete the exercise.".__(
                async () =>
                {
                    var completeWorkoutResponse = await workoutClient.UpdatePOSTAsync(userId);
                    completeWorkoutResponse.Week.Should().Be(2);
                    completeWorkoutResponse.Day.Should().Be(1);
                });
        }

        [StepTest, Order(200)]
        public async Task LinearProgression_AllExercisesDelete()
        {
            await "The exercises should all be deleted.".__(async () =>
            {
                var deleteResponse = await workoutClient.DeleteAllAsync(userId);
                deleteResponse.Should().BeTrue();
            });
        }

        [StepTest, Order(210)]
        public async Task User_WeekAndDaySetTo1()
        {
            await "The user's week and day should reset to 1.".__(async () =>
            {
                var userWeekAndDAyResponse = await workoutClient.WeekAndDayAsync(userId);
                userWeekAndDAyResponse.Week.Should().NotBe(1);
                userWeekAndDAyResponse.Day.Should().Be(1);

                var request = new UpdateWeekAndDayRequest()
                {
                    Day = 1,
                    UserId = userId,
                    Week = 1
                };
                var updateWeekAndDayResponse = await workoutClient.UpdateWeekAndDayAsync(request);

                updateWeekAndDayResponse.Week.Should().Be(1);
                updateWeekAndDayResponse.Day.Should().Be(1);
            });
        }

        public CreateLinearProgressionExerciseRequest BuildLinearProgressionExerciseRequest(
            string exerciseName, Category category, EquipmentType equipmentType, int liftDay,
            int liftOrder, decimal startingWeight, decimal weightProgression, int attemptsBeforeDeload,
            bool primaryExercise, int week, int minimumReps, int maximumReps, int targetSets,
            int weightIndex, EquipmentStackKey equipmentStackKey, Guid userId)
        {
            var exercise = new ExerciseBuilder()
                .WithDefaultValues()
                .WithExerciseName(exerciseName)
                .WithCategory((OperationStacked.Enums.Category)category)
                .WithEquipmentType((OperationStacked.Enums.EquipmentType)equipmentType)
                .WithUserId(userId)
                .Build();

            var workoutExercise = new WorkoutExerciseBuilder()
                .WithDefaultValues()
                .WithExerciseId(exercise.Id) // Assuming this should be set here
                .WithLiftDay(liftDay)
                .WithLiftOrder(liftOrder)
                .Build();

            var linearProgressionExercise = new LinearProgressionExerciseBuilder()
                .WithDefaultValues()
                .WithReps(minimumReps, maximumReps)
                .WithSets(targetSets)
                .WithWeightProgression(weightProgression)
                .WithWorkingWeight(startingWeight);

            // Assuming AdaptToCreateRequest is correctly mapping the LinearProgressionExercise to CreateLinearProgressionExerciseRequest
            return linearProgressionExercise.AdaptToCreateRequest(workoutExercise.AdaptToCreateRequest());
        }

    }
}
