// using Bogus;
// using FluentAssertions;
// using Newtonsoft.Json;
// using NUnit.Framework;
// using OperationStacked.Models;
// using OperationStacked.Requests;
// using System.Net.Http.Json;
// using MoreLinq;
// using OperationStacked.Enums;
//
// namespace OperationStackedTests.Functional
// {
//     [TestFixture]
//     public class ExerciseCreation : BaseApiTest
//     {
//         private const string url = "/workout-creation";
//         
//         [Test]
//         public async Task LinearProgressionExerciseCreates()
//         {
//             // Act
//             var exercises = new Faker<CreateExerciseModel>()
//                 
//                 .RuleFor(x => x.Category, "Legs")
//                 .RuleFor(x => x.ExerciseName, "Squats")
//                 .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
//                 .RuleFor(x => x.Username, "ChickenLegTim")
//                 .RuleFor(x => x.LiftDay, 1)
//                 .RuleFor(x => x.LiftOrder, 1)
//                 .RuleFor(x => x.UserId, "1")
//                 .RuleFor(x => x.StartingWeight, 40)
//                 .RuleFor(x => x.WeightIndex, 0)
//                 .RuleFor(x => x.TargetSets, 3)
//                 .RuleFor(x => x.MinimumReps, 8)
//                 .RuleFor(x => x.MaximumReps, 12)
//                 .RuleFor(x => x.EquipmentType, EquipmentType.Barbell)
//                 .Generate(1);
//
//               
//             var request = new CreateWorkoutRequest();
//             request.ExerciseDaysAndOrders = exercises;
//             request.userId = "1";
//
//             var response = await _client.PostAsJsonAsync(url
//                , request);
//
//             // Assert
//             response.EnsureSuccessStatusCode(); // Status Code 200-299
//             _context.Exercises.Count().Should().Be(request.ExerciseDaysAndOrders.Count);
//             _context.Exercises.ForEach(x => x.Template.Should().Be(ExerciseTemplate.LinearProgression));
//             _context.Exercises.ForEach(x => x.EquipmentType.Should().Be(EquipmentType.Barbell));
//             _context.Exercises.ForEach(x => x.Username.Should().Be("ChickenLegTim"));
//             _context.LinearProgressionExercises.ForEach(x => x.Username.Should().Be("ChickenLegTim"));
//             _context.LinearProgressionExercises.ForEach(x => x.MaximumReps.Should().Be(12));
//             _context.LinearProgressionExercises.Count().Should().Be(request.ExerciseDaysAndOrders.Count);
//             //var getRequest = url + "/1/1/1";
//             //var getResponse = await _client.GetFromJsonAsync<ExerciseViewModel>(getRequest);
//             ////var x = JsonConvert.SerializeObject<List<ExerciseViewModel>>(getResponse);
//
//         }
//
//         [Test]
//         public async Task A2SHypertrophyExerciseCreates()
//         {
//             // Act
//             var exercises = new Faker<CreateExerciseModel>()
//
//                 .RuleFor(x => x.Category, "Legs")
//                 .RuleFor(x => x.ExerciseName, "Squats")
//                 .RuleFor(x => x.Template, ExerciseTemplate.A2SHypertrophy)
//                 .RuleFor(x => x.Username, "ChickenLegTim")
//                 .RuleFor(x => x.PrimaryLift, false)
//                 .RuleFor(x => x.LiftOrder, 1)
//                 .RuleFor(x => x.UserId, "1")
//                 .RuleFor(x => x.TrainingMax, 40)
//                 .RuleFor(x => x.EquipmentType, EquipmentType.Barbell)
//                 .RuleFor(x => x.Block, A2SBlocks.Hypertrophy)
//                 .RuleFor(x => x.WeightProgression, 2.5m)
//                 .Generate(1);
//
//
//             var request = new CreateWorkoutRequest();
//             request.ExerciseDaysAndOrders = exercises;
//             request.userId = "1";
//
//             var response = await _client.PostAsJsonAsync(url
//                , request);
//
//             // Assert
//             response.EnsureSuccessStatusCode(); // Status Code 200-299
//             _context.Exercises.Count().Should().Be(request.ExerciseDaysAndOrders.Count);
//             _context.Exercises.ForEach(x => x.Template.Should().Be(ExerciseTemplate.A2SHypertrophy));
//             _context.Exercises.ForEach(x => x.EquipmentType.Should().Be(EquipmentType.Barbell));
//             _context.Exercises.ForEach(x => x.Username.Should().Be("ChickenLegTim"));
//             _context.A2SHypertrophyExercises.ForEach(x => x.PrimaryLift.Should().Be(false));
//             //Return 0.60m for week 0,
//             _context.A2SHypertrophyExercises.ForEach(x => x.Intensity.Should().Be(0.60m));
//             _context.A2SHypertrophyExercises.ForEach(x => x.TrainingMax.Should().Be(40));
//             _context.A2SHypertrophyExercises.ForEach(x => x.Block.Should().Be(A2SBlocks.Hypertrophy));
//
//             //Return 3 for week 0,
//             _context.A2SHypertrophyExercises.ForEach(x => x.Sets.Should().Be(3));
//             //Return 7 for week 0,
//             _context.A2SHypertrophyExercises.ForEach(x => x.RepsPerSet.Should().Be(7));
//             //really intensity/TrainingMax/Sets should have been a unit test 
//             _context.A2SHypertrophyExercises.Count().Should().Be(request.ExerciseDaysAndOrders.Count);
//
//         }
//
//         [TearDown]
//         public void Teardown()
//         {
//             _context.Database.EnsureDeleted();
//         }
//     }
// }
