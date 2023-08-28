// using Concise.Steps;
// using FluentAssertions;
// using NUnit.Framework;
// using OperationStacked.TestLib;
// using OperationStackedAuth.Tests;
//
// namespace OperationStackedTests.Functional;
//
// public class ExerciseTests
// {
//     public Guid UserId;
//     public WorkoutClient WorkoutClient;
//     public List<Exercise> Workout;
//
//     [OneTimeSetUp]
//     public async Task InitiateClient()
//     {
//         var (client, _userId) = await ApiClientFactory.CreateWorkoutClientAsync(true);
//         WorkoutClient = client;
//         UserId = _userId;
//     }
//     
//     [StepTest, Order(10)]
//     public async Task Exercise_WorkingWeightShouldBePopulated()
//     {
//         await "Create a workout with one exercise".__(async () =>
//         {
//             var workoutRequest = new CreateWorkoutRequest()
//             {
//                 UserId = UserId,
//                 ExerciseDaysAndOrders = new List<CreateExerciseModel>()
//                 {
//                     new CreateExerciseModel()
//                     {
//                         Template = ExerciseTemplate._0,
//                         LiftDay = 1,
//                         LiftOrder = 1,
//                         UserId = UserId,
//                         EquipmentType = EquipmentType._0,
//                         ExerciseName = "Back Squat",
//                         Category = "Legs",
//                         MinimumReps = 8,
//                         MaximumReps = 12,
//                         TargetSets = 3,
//                         PrimaryExercise = true,
//                         StartingWeight = 10.00,
//                         WeightProgression = 5,
//                         AttemptsBeforeDeload = 2,
//                         Week = 1
//                     }
//                 }
//             };
//             
//             var response = await WorkoutClient.WorkoutCreationPOSTAsync(workoutRequest);
//             Workout = response.Exercises.ToList();
//         });
//     }
//
//     [StepTest, Order(20)]
//     public async Task Exercise_WorkingWeightShouldUpdateManually()
//     {
//         await "Working weight is manually updated.".__(async () =>
//         {
//             var exercise = Workout.First();
//             var originalWeight = exercise.WorkingWeight;
//             originalWeight.Should().Be(10);
//             var updateResponse = await WorkoutClient.WorkoutCreationPUTAsync(exercise.Id, 15);
//
//             updateResponse.Should().Be(true);
//             var updatedExercise = await WorkoutClient.WorkoutCreationGET2Async(exercise.Id);    
//             updatedExercise.Exercises.WorkingWeight.Should().Be(15);    
//
//         });
//         
//     }
//
//     [OneTimeTearDown]
//     public async Task CleanUp()
//     {
//         await WorkoutClient.DeleteAllAsync(UserId);
//     }
// }