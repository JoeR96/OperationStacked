// using System.Net.Http.Json;
// using Bogus;
// using FluentAssertions;
// using Microsoft.EntityFrameworkCore;
// using NUnit.Framework;
// using OperationStacked.Entities;
// using OperationStacked.Requests;
//
// namespace OperationStackedTests.Functional
// {
//     [TestFixture]
//
//     public class A2SHypertrophyTests : BaseApiTest
//     {
//         private const string completeExerciseUrl = "/workout-creation/complete";
//
//         //2x Decrease Training Max
//         //1x Stays Same
//         //2X Increase Training Max
//         //1x Next Week block changes
//         [Test]
//         [TestCase(3,95,70)]
//         [TestCase(4, 98, 75)]
//         [TestCase(5, 100, 75)]
//         [TestCase(6, 100.5, 75)]
//         [TestCase(7, 101, 75)]
//         [TestCase(8, 101.5, 75)]
//         [TestCase(9, 102, 75)]
//         [TestCase(10, 103, 75)]
//         public async Task TrainingMaxCalculatesForA2SHypertrophyBlockPrimary(int reps,decimal expectedTrainingMax, decimal expectedWorkingWeight)
//         {
//             var exercise = FakeExercise().Generate();
//             
//             _context.Exercises.Add(exercise);
//             _context.SaveChanges();
//
//             var request = new CompleteExerciseRequest()
//             {
//                 Id = exercise.Id,
//                 Reps = new int[] { reps },
//                 Sets = 3
//             };
//             var response = await _client.PostAsJsonAsync(completeExerciseUrl
//                , request);
//             var nextWeek = _context.A2SHypertrophyExercises.Where(x => x.Id != request.Id).FirstOrDefault();
//             nextWeek?.TrainingMax.Should().Be(expectedTrainingMax);
//             nextWeek?.WorkingWeight.Should().Be(expectedWorkingWeight);
//         }
//
//         public async Task HypertrophyBlockProceedsToStrengthBlock()
//         {
//             var exercise = FakeExercise().Generate();
//             exercise.Week = 5;
//
//             _context.Exercises.Add(exercise);
//             _context.SaveChanges();
//
//             var request = new CompleteExerciseRequest()
//             {
//                 Id = exercise.Id,
//                 Reps = new int[] { 5 },
//                 Sets = 3
//             };
//
//             var response = await _client.PostAsJsonAsync(completeExerciseUrl
//                , request);
//
//             var nextWeek = _context.A2SHypertrophyExercises.Where(x => x.Id != request.Id).FirstOrDefault();
//             nextWeek.Block.Should().Be(OperationStacked.Enums.A2SBlocks.Strength);
//             nextWeek.Week.Should().Be(0);
//
//         }
//         private static Faker<A2SHypertrophyExercise> FakeExercise() =>
//             new Faker<A2SHypertrophyExercise>()
//             .RuleFor(i => i.Template, OperationStacked.Models.ExerciseTemplate.A2SHypertrophy)
//             .RuleFor(i => i.Category, "Legs")
//             .RuleFor(i => i.Username, "BigDaveTV")
//             .RuleFor(i => i.ExerciseName, "Squats")
//             .RuleFor(i => i.AmrapRepTarget, 5)
//             .RuleFor(i => i.TrainingMax, 100.00m)
//             .RuleFor(i => i.PrimaryLift, true)
//             .RuleFor(i => i.RoundingValue, 5m)
//             .RuleFor(i => i.UserId, "HELLO");
//         [TearDown]
//         public void Teardown()
//         {
//             _context.Database.EnsureDeleted();
//         }
//     }
// }
