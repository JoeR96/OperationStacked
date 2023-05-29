using Concise.Steps;
using FluentAssertions;
using NUnit.Framework;
using OperationStacked.Entities;
using OperationStacked.TestLib;
using OperationStackedAuth.Tests;
using EquipmentStack = OperationStacked.TestLib.EquipmentStack;
using Exercise = OperationStacked.TestLib.Exercise;

namespace OperationStackedTests.Functional
{
    [TestFixture]
    [NonParallelizable]
    public class LinearProgressionProgressTests 
    {
        public Guid userId;
        public WorkoutClient workoutClient;
        public List<Exercise> workout;

        [OneTimeSetUp]
        public async Task InitiateClient()
        {
            var (client, _userId) = await ApiClientFactory.CreateWorkoutClientAsync(true);
            workoutClient = client;
            userId = _userId;
        }
        [StepTest, Order(1)]
        public async Task LinearProgression_WorkoutCreatesWithAllEquipmentTypes()
        {
           
            // var createEquipmentStackRequest = new CreateEquipmentStackRequest()
            // {
            //     StartWeight = 1.10,
            //     IncrementValue = 2.30,
            //     IncrementCount = 18,
            //     InitialIncrements = new decimal[] {  },
            //     EquipmentStackKey = "TheGymGroupCable",
            //     UserID = userId
            // };
            // await "The stack is created is retrieved".__(async () =>
            // {
            //     var createResponse = await workoutClient.CreateAsync(createEquipmentStackRequest);
            //    
            //     var response = await workoutClient.EquipmentStackGETAsync( createResponse.Stack.Id);
            //
            //     response.Stack.Id.Should().Be(createResponse.Stack.Id);
            // });
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
                        ExerciseName = "Back Squat",
                        Category = "Legs",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = true,
                        StartingWeight = 10.00,
                        WeightProgression = 5,
                        AttemptsBeforeDeload = 2,
                        Week = 1
                     },
                    new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 1,
                        LiftOrder = 2,
                        UserId = userId,
                        EquipmentType = EquipmentType._1,
                        ExerciseName = "Incline Smith Press",
                        Category = "Legs",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        StartingWeight = 10,
                        WeightProgression = 2.5,
                        AttemptsBeforeDeload = 2
                    },
                    new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 1,
                        LiftOrder = 3,
                        UserId = userId,
                        EquipmentType = EquipmentType._2,
                        ExerciseName = "Bicep Curl",
                        Category = "Arms",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        StartingWeight = 8.00,
                        AttemptsBeforeDeload = 2
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
                        StartingWeight = 10.00,
                        AttemptsBeforeDeload = 2
                     },
                     new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 1,
                        LiftOrder = 5,
                        UserId = userId,
                        EquipmentType = EquipmentType._3,
                        ExerciseName = "Low Row",
                        Category = "Back",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        AttemptsBeforeDeload = 2,
                        WeightIndex = 1,
                        EquipmentStack = new CreateEquipmentStackRequest()
                        {
                            StartWeight = 4.5,
                            IncrementValue = 7.00,
                            IncrementCount = 15,
                            InitialIncrements = new decimal[] { 6.5m },
                            UserID = userId,
                            EquipmentStackKey = "The Gym Low Row"
                        }
                    },
                     new CreateExerciseModel()
                    {
                        Template = ExerciseTemplate._0,
                        LiftDay = 1,
                        LiftOrder = 6,
                        UserId = userId,
                        EquipmentType = EquipmentType._4,
                        ExerciseName = "Pec Fly",
                        Category = "Chest",
                        MinimumReps = 8,
                        MaximumReps = 12,
                        TargetSets = 3,
                        PrimaryExercise = false,
                        AttemptsBeforeDeload = 2,
                        WeightIndex = 1,
                        EquipmentStackKey = EquipmentStackKey._2
                    },
                }
            };

            await "The exercise is created and returned.".__(async () =>
            {
                var createWorkoutResponse = await workoutClient.WorkoutCreationPOSTAsync(request);
                createWorkoutResponse.Exercises.Count.Should().Be(6);
            });
        }

        [StepTest, Order(2)]
        public async Task LinearProgression_WorkoutIsRetrieved()
        {
            await "The workout is retrieved".__(async () =>
            {
                var getWorkoutResponse = await workoutClient.WorkoutCreationGETAsync(userId, 1, 1, false);
                workout = getWorkoutResponse.Exercises.ToList();

                workout.Count().Should().Be(6);
            });
        }
        [StepTest, Order(3)]
        public async Task LinearProgression_BarbellExerciseProgresses()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    Id = workout[0].Id,
                    Reps = new int[]{12,12,12},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(15);
                completionResponse.Exercise.ParentId.Should().Be(workout[0].Id);
            });
        }
        [StepTest, Order(4)]
        public async Task LinearProgression_SmithMachineExerciseProgresses()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    Id = workout[1].Id,
                    Reps = new int[]{12,12,12},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(12.5);
                completionResponse.Exercise.ParentId.Should().Be(workout[1].Id);
            });
        }

        [StepTest, Order(5)]
        public async Task LinearProgression_DumbbellProgressesBy1KGWhenWorkingWeightIsBelow9KG()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    Id = workout[2].Id,
                    Reps = new int[]{12,12,12},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(9);
                completionResponse.Exercise.ParentId.Should().Be(workout[2].Id);
            });
        }
        
        [StepTest, Order(6)]
        public async Task LinearProgression_DumbbellProgressesBy2KGWhenWorkingWeightIsAbove9KG()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    Id = workout[3].Id,
                    Reps = new int[]{12,12,12},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(12);
                completionResponse.Exercise.ParentId.Should().Be(workout[3].Id);
            });
        }

        [StepTest, Order(7)]
        public async Task LinearProgression_MachineIndexProgress()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    Id = workout[4].Id,
                    Reps = new int[]{12,12,12},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(11);
                completionResponse.Exercise.ParentId.Should().Be(workout[4].Id);
                
            });
        }

        [StepTest, Order(8)]
        public async Task LinearProgression_CableIndexProgress()
        {
            await "The exercise is completed and the response is the generated exercise for next week,".__(async () =>
            {
                var completionResponse = await workoutClient.CompleteAsync(new CompleteExerciseRequest()
                {
                    Id = workout[5].Id,
                    Reps = new int[]{12,12,12},
                    Sets = 3
                });
                
                completionResponse.Exercise.LiftWeek.Should().Be(2);
                completionResponse.Exercise.WorkingWeight.Should().Be(3.40);
                completionResponse.Exercise.ParentId.Should().Be(workout[5].Id);
            });
        }
        [StepTest, Order(9)]
        public async Task LinearProgression_AllExercisesDelete()
        {
            await "The exercises should all be deleted.".__(async () =>
             {
                 var deleteResponse = await workoutClient.DeleteAllAsync(userId);
                 deleteResponse.Should().BeTrue();
            });
        }
    }
}

