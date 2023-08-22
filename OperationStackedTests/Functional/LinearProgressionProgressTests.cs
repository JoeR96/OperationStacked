using Concise.Steps;
using FluentAssertions;
using NUnit.Framework;
using OperationStacked.TestLib;
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
        public List<Exercise> Workout;

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
                StartWeight = (decimal)1.1,
                InitialIncrements = new decimal?[] { 1.1m },
                IncrementValue = (decimal)2.3,
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
            var request = new CreateWorkoutRequest
            {
                UserId = userId,
                ExerciseDaysAndOrders = new List<CreateExerciseModel>()
                {
                    new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 1,
                        LiftOrder = 1,
                        UserId = userId,
                        EquipmentType = EquipmentType._0,
                        ExerciseName = "Bench Press",
                        Category = "Chest",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = true,
                        StartingWeight = 60.00,
                        WeightProgression = 2.50,
                        AttemptsBeforeDeload = 2,
                        Week = 1
                     },
                    new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 1,
                        LiftOrder = 2,
                        UserId = userId,
                        EquipmentType = EquipmentType._0,
                        ExerciseName = "Barbell Row",
                        Category = "Back",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        StartingWeight = 60,
                        WeightProgression = 2.5,
                        Week = 1
                    },
                    new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 1,
                        LiftOrder = 3,
                        UserId = userId,
                        EquipmentType = EquipmentType._1,
                        ExerciseName = "Incline Smith Press",
                        Category = "Shoulders",
                        MinimumReps = 8,
                        MaximumReps = 10,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        StartingWeight = 30.00,
                        AttemptsBeforeDeload = 2,
                        WeightProgression = 2.5,
                        Week = 1
                    },
                    new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 1,
                        LiftOrder = 4,
                        UserId = userId,
                        EquipmentType = EquipmentType._2,
                        ExerciseName = "Hammer Curl",
                        Category = "Arms",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        StartingWeight = 12.00,
                        AttemptsBeforeDeload = 2,
                        Week = 1
                     },
                     new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 1,
                        LiftOrder = 5,
                        UserId = userId,
                        EquipmentType = EquipmentType._4,
                        ExerciseName = "Tricep PullDown",
                        Category = "Tricep",
                        MinimumReps = 12,
                        MaximumReps = 15,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        AttemptsBeforeDeload = 2,
                        WeightIndex = 5,
                        EquipmentStackKey = EquipmentStackKey._2,
                        Week = 1
                    },
                     new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 1,
                        LiftOrder = 6,
                        UserId = userId,
                        EquipmentType = EquipmentType._2,
                        ExerciseName = "Lateral Raise",
                        Category = "Shoulder",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 4,
                        PrimaryExercise = false,
                        AttemptsBeforeDeload = 2,
                        WeightIndex = 7,
                        EquipmentStackKey = EquipmentStackKey._2,
                        Week = 1,
                    },
                     new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 2,
                        LiftOrder = 1,
                        UserId = userId,
                        EquipmentType = EquipmentType._0,
                        ExerciseName = "Back Squat",
                        Category = "Legs",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = true,
                        StartingWeight = 60.00,
                        WeightProgression = 5,
                        AttemptsBeforeDeload = 2,
                        Week = 1
                     },
                    new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 2,
                        LiftOrder = 2,
                        UserId = userId,
                        EquipmentType = EquipmentType._0,
                        ExerciseName = "Romanian Deadlift",
                        Category = "Legs",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        StartingWeight = 80,
                        WeightProgression = 5,
                        Week = 1
                    },
                    new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 2,
                        LiftOrder = 3,
                        UserId = userId,
                        EquipmentType = EquipmentType._3,
                        ExerciseName = "Leg Extension",
                        Category = "Arms",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        AttemptsBeforeDeload = 2,
                        Week = 1,
                        EquipmentStack = new CreateEquipmentStackRequest()
                            {
                                StartWeight = (decimal)4.5,
                                InitialIncrements = new decimal?[] { },
                                IncrementValue = (decimal)4.3,
                                IncrementCount = 20,
                                EquipmentStackKey = "The Gym Leg Extension"
                            }
                      },
                     new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 2,
                        LiftOrder = 4,
                        UserId = userId,
                        EquipmentType = EquipmentType._2,
                        ExerciseName = "Hammer Curl",
                        Category = "Arms",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        StartingWeight = 12.00,
                        AttemptsBeforeDeload = 2,
                        Week = 1
                     },
                    //day3
                    new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 3,
                        LiftOrder = 1,
                        UserId = userId,
                        EquipmentType = EquipmentType._4,
                        ExerciseName = "Unilateral Pull Down",
                        Category = "Back",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        EquipmentStackKey = EquipmentStackKey._2,
                        AttemptsBeforeDeload = 2,
                        WeightIndex = 9,
                        Week = 1
                     },
                     new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 3,
                        LiftOrder = 2,
                        UserId = userId,
                        EquipmentType = EquipmentType._0,
                        ExerciseName = "Overhead Press",
                        Category = "Shoulders",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        AttemptsBeforeDeload = 2,
                        StartingWeight = 35,
                        Week = 1,
                        WeightProgression = 2.5,
                    },
                     new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 3,
                        LiftOrder = 3,
                        UserId = userId,
                        EquipmentType = EquipmentType._4,
                        ExerciseName = "Unilateral Pull Down",
                        Category = "Back",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        AttemptsBeforeDeload = 2,
                        WeightIndex = 1,
                        EquipmentStackKey = EquipmentStackKey._2,
                        Week = 1,
                    },
                     new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 3,
                        LiftOrder = 4,
                        UserId = userId,
                        EquipmentType = EquipmentType._2,
                        ExerciseName = "Lateral Raise",
                        Category = "Shoulders",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        StartingWeight = 6.00,
                        AttemptsBeforeDeload = 2,
                        Week = 1
                     },
                    new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 3,
                        LiftOrder = 5,
                        UserId = userId,
                        EquipmentType = EquipmentType._2,
                        ExerciseName = "Bicep Curl",
                        Category = "Arms",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        StartingWeight = 12.00,
                        AttemptsBeforeDeload = 2,
                        Week = 1
                     },
                     new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 3,
                        LiftOrder = 6,
                        UserId = userId,
                        EquipmentType = EquipmentType._4,
                        ExerciseName = "Unilateral Tricep PullDown",
                        Category = "Tricep",
                        MinimumReps = 12,
                        MaximumReps = 15,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        AttemptsBeforeDeload = 2,
                        WeightIndex = 2,
                        EquipmentStackKey = EquipmentStackKey._2,
                        Week = 1
                    },
                      new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 4,
                        LiftOrder = 1,
                        UserId = userId,
                        EquipmentType = EquipmentType._2,
                        ExerciseName = "Bulgarian Split Squat",
                        Category = "Legs",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        StartingWeight = 10,
                        Week = 1
                    },
                        new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 4,
                        LiftOrder = 2,
                        UserId = userId,
                        EquipmentType = EquipmentType._0,
                        ExerciseName = "Leg Press",
                        Category = "Legs",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        StartingWeight = 80,
                        WeightProgression = 10,
                        Week = 1
                    },
                    new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 4,
                        LiftOrder = 3,
                        UserId = userId,
                        EquipmentType = EquipmentType._3,
                        ExerciseName = "Leg Curl",
                        Category = "Arms",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        AttemptsBeforeDeload = 2,
                        Week = 1,
                        EquipmentStack = new CreateEquipmentStackRequest()
                            {
                                StartWeight = (decimal)4.5,
                                InitialIncrements = new decimal?[] { },
                                IncrementValue = (decimal)4.3,
                                IncrementCount = 20,
                                EquipmentStackKey = "The Gym Leg Curl"
                            }
                      },
                    new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 4,
                        LiftOrder = 4,
                        UserId = userId,
                        EquipmentType = EquipmentType._2,
                        ExerciseName = "Bicep Curl",
                        Category = "Arms",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        StartingWeight = 12.00,
                        AttemptsBeforeDeload = 2,
                        Week = 1
                     },
                }
            };
            WorkoutCreationResult t;
            await "The exercise is created and returned.".__(async () =>
            {
                t = await workoutClient.WorkoutCreationPOSTAsync(request);
                t.Exercises.Count.Should().Be(12);
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
                    exercise.LiftWeek.Should().Be(1);
                }
                Workout.Count().Should().Be(6);
                Workout.ForEach(e => e.LiftDay.Should().Be(1));
                Workout.ForEach(e => e.LiftWeek.Should().Be(1));
                Workout.ForEach(e => e.WorkingWeight.Should().BeGreaterThan(0));

            });
        }
        [StepTest, Order(30)]
        public async Task LinearProgression_BarbellExerciseProgresses()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    Id = Workout[0].Id,
                    Reps = new int[]{12,12,12},
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
                    Id = Workout[1].Id,
                    Reps = new int[]{12,12,12},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(12.5);
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
                    Id = Workout[2].Id,
                    Reps = new int[]{12,12,12},
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
                    Id = Workout[3].Id,
                    Reps = new int[]{12,12,12},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(12);
                completionResponse.Exercise.ParentId.Should().Be(Workout[3].Id);
            });
        }

        [StepTest, Order(70)]
        public async Task  LinearProgression_MachineIndexProgress()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    Id = Workout[4].Id,
                    Reps = new int[]{12,12,12},
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
                    Id = Workout[5].Id,
                    Reps = new int[]{12,12,12},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(5.70);
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
                    var completeWorkoutResponse = await workoutClient.UpdateAsync(userId);
                    
                    completeWorkoutResponse.Week.Should().Be(1);
                    completeWorkoutResponse.Day.Should().Be(2);
                });
        }

        private List<Exercise> WorkoutTwo;
        
        [StepTest, Order(110)]
        public async Task LinearProgression_WorkoutTwoIsRetrieved()
        {
            await "The workout is retrieved".__(async () =>
            {
                var getWorkoutResponse = await workoutClient.WorkoutCreationGETAsync(userId, 1, 2, false);
                WorkoutTwo = getWorkoutResponse.Exercises.ToList();

                foreach (var exercise in Workout)
                {
                    exercise.LiftWeek.Should().Be(1);
                }
                WorkoutTwo.Count().Should().Be(6);
                
                WorkoutTwo.ForEach(e => e.LiftWeek.Should().Be(1));
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
                    Id = WorkoutTwo[0].Id,
                    Reps = new int[]{8,8,8},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(WorkoutTwo[0].WorkingWeight);
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
                    Id = WorkoutTwo[1].Id,
                    Reps = new int[]{11,11,11},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(WorkoutTwo[1].WorkingWeight);
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
                    Id = WorkoutTwo[2].Id,
                    Reps = new int[]{11,11,11},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(WorkoutTwo[2].WorkingWeight);
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
                    Id = WorkoutTwo[3].Id,
                    Reps = new int[]{11,11,11},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(WorkoutTwo[3].WorkingWeight);
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
                    Id = WorkoutTwo[4].Id,
                    Reps = new int[]{11,11,11},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(WorkoutTwo[4].WorkingWeight);
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
                    Id = WorkoutTwo[5].Id,
                    Reps = new int[]{11,11,11},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(WorkoutTwo[5].WorkingWeight);
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
                    var completeWorkoutResponse = await workoutClient.UpdateAsync(userId);                    
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
    }
}

