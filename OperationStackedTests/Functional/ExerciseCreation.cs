using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStackedTests.Helpers;
using System.Net.Http.Json;

namespace OperationStackedTests.Functional
{
    [TestFixture]
    public class ExerciseCreation : BaseApiTest
    {
        private const string url = "/workout-creation";
        private const string completeExerciseUrl = "/workout-creation/complete";


        [Test]
        public async Task ExerciseCreateAsync()
        {

            var client = _application.CreateClient();
            // Act
            var exercises = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Legs")
                .RuleFor(x => x.ExerciseName, "Squats")
                .RuleFor(x => x.Template, "LinearProgression")
                .RuleFor(x => x.Username, "ChickenLegTim")
                .RuleFor(x => x.LiftDay, 1)
                .RuleFor(x => x.LiftOrder, 1)
                .RuleFor(x => x.UserId, 1)
                .Generate(15);

            var request = new CreateWorkoutRequest();
            request.ExerciseDaysAndOrders = exercises;
            request.userId = 1;

            var response = await client.PostAsJsonAsync(url
               , request);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            _context.Exercises.Count().Should().Be(request.ExerciseDaysAndOrders.Count);

            var get = new GetWorkoutRequest()
            {
                Day = 1,
                Week = 1,
                UserId = 1
            };
            var getRequest = url + "/1/1/1";
            var getResponse = await client.GetStringAsync(getRequest);
            var x = JsonConvert.SerializeObject(getResponse);

        }

        [Test]
        public async Task ExerciseProgressesWhenSetCountAndMaximumRepCountAreMet()
        {
            var id = Guid.NewGuid();
            Faker<LinearProgressionExercise> exercise = FakeExercise(id);

            _context.Exercises.Add(exercise);
            _context.SaveChanges();
            // Act          
            var request = new CompleteExerciseRequest()
            {
                Id = id,
                Reps = 12,
                Sets = 5
            };

            var response = await _client.PostAsJsonAsync(completeExerciseUrl
               , request);
            var result = _context.LinearProgressionExercises.Where(x => x.ParentId == id).FirstOrDefault();
            result.LiftWeek.Should().Be(2);
            result.WeightIndex.Should().Be(2);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

        }

        private static Faker<LinearProgressionExercise> FakeExercise(Guid id)
        {
            return new Faker<LinearProgressionExercise>()
                            .RuleFor(i => i.MaximumReps, 12)
                            .RuleFor(i => i.MinimumReps, 8)
                            .RuleFor(i => i.TargetSets, 5)
                            .RuleFor(i => i.StartingSets, 3)
                            .RuleFor(i => i.CurrentSets, 5)
                            .RuleFor(i => i.Id, id)
                            .RuleFor(i => i.ExerciseName, "Squats")
                            .RuleFor(i => i.Category, "Legs")
                            .RuleFor(i => i.Username, "Tim420")
                            .RuleFor(i => i.WeightIndex, 1);
        }

        [Test]
    public async Task ExercisesFailsFirstTimeSetsOnly()
    {
         
           var id = Guid.NewGuid();
            var exercise = new Faker<LinearProgressionExercise>()
                .RuleFor(i => i.MaximumReps, 12)
                .RuleFor(i => i.MinimumReps, 8)
                .RuleFor(i => i.TargetSets, 5)
                .RuleFor(i => i.StartingSets, 3)
                .RuleFor(i => i.CurrentSets, 5)
                .RuleFor(i => i.Id, id)
                .RuleFor(i => i.ExerciseName, "Squats")
                .RuleFor(i => i.Category, "Legs")
                .RuleFor(i => i.Username, "Tim420")
                .RuleFor(i => i.WeightIndex, 1)
                .RuleFor(i => i.CurrentAttempt, 1)
                .RuleFor(i => i.AttemptsBeforeDeload, 2);

           _context.Exercises.Add(exercise);
           _context.SaveChanges();
           // Act          
           var request = new CompleteExerciseRequest()
           {
               Id = id,
               Reps = 12,
               Sets = 4
           };

           var response = await _client.PostAsJsonAsync(completeExerciseUrl
              , request);
           var result = _context.LinearProgressionExercises.Where(x => x.ParentId == id).FirstOrDefault();

           result.LiftWeek.Should().Be(2);
           result.WeightIndex.Should().Be(1);
           result.CurrentAttempt.Should().Be(2);
           response.EnsureSuccessStatusCode(); // Status Code 200-299
     
        }

        [Test]
        public async Task ExercisesFailsRepsAndSets()
        {
            var id = Guid.NewGuid();
            var exercise = new Faker<LinearProgressionExercise>()
                .RuleFor(i => i.MaximumReps, 12)
                .RuleFor(i => i.MinimumReps, 8)
                .RuleFor(i => i.TargetSets, 5)
                .RuleFor(i => i.StartingSets, 3)
                .RuleFor(i => i.CurrentSets, 5)
                .RuleFor(i => i.Id, id)
                .RuleFor(i => i.ExerciseName, "Squats")
                .RuleFor(i => i.Category, "Legs")
                .RuleFor(i => i.Username, "Tim420")
                .RuleFor(i => i.WeightIndex, 1)
                .RuleFor(i => i.CurrentAttempt, 1)
                .RuleFor(i => i.AttemptsBeforeDeload, 2);

            _context.Exercises.Add(exercise);
            _context.SaveChanges();
            // Act          
            var request = new CompleteExerciseRequest()
            {
                Id = id,
                Reps = 7,
                Sets = 4
            };
            var response = await _client.PostAsJsonAsync(completeExerciseUrl
               , request);
            var result = _context.LinearProgressionExercises.Where(x => x.ParentId == id).FirstOrDefault();
            result.LiftWeek.Should().Be(2);
            result.WeightIndex.Should().Be(1);
            result.CurrentAttempt.Should().Be(2);
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            // Assert



        }

        [Test]
        public async Task ExercisesFailsFirstTimeRepsOnly()
        {
            var id = Guid.NewGuid();
            var exercise = new Faker<LinearProgressionExercise>()
                .RuleFor(i => i.MaximumReps, 12)
                .RuleFor(i => i.MinimumReps, 8)
                .RuleFor(i => i.TargetSets, 5)
                .RuleFor(i => i.StartingSets, 3)
                .RuleFor(i => i.CurrentSets, 5)
                .RuleFor(i => i.Id, id)
                .RuleFor(i => i.ExerciseName, "Squats")
                .RuleFor(i => i.Category, "Legs")
                .RuleFor(i => i.Username, "Tim420")
                .RuleFor(i => i.WeightIndex, 1)
                .RuleFor(i => i.CurrentAttempt, 1)
                .RuleFor(i => i.AttemptsBeforeDeload, 2);

            _context.Exercises.Add(exercise);
            _context.SaveChanges();
            // Act          
            var request = new CompleteExerciseRequest()
            {
                Id = id,
                Reps = 7,
                Sets = 5
            };
            var response = await _client.PostAsJsonAsync(completeExerciseUrl
               , request);
            var result = _context.LinearProgressionExercises.Where(x => x.ParentId == id).FirstOrDefault();

            result.LiftWeek.Should().Be(2);
            result.WeightIndex.Should().Be(1);
            result.CurrentAttempt.Should().Be(2);
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            // Assert

        }
        [Test]
        public async Task ExerciseDeloadsAfterfail()
        {
            var id = Guid.NewGuid();
            var exercise = new Faker<LinearProgressionExercise>()
                .RuleFor(i => i.MaximumReps, 12)
                .RuleFor(i => i.MinimumReps, 8)
                .RuleFor(i => i.TargetSets, 5)
                .RuleFor(i => i.StartingSets, 3)
                .RuleFor(i => i.CurrentSets, 5)
                .RuleFor(i => i.Id, id)
                .RuleFor(i => i.ExerciseName, "Squats")
                .RuleFor(i => i.Category, "Legs")
                .RuleFor(i => i.Username, "Tim420")
                .RuleFor(i => i.WeightIndex, 2)
                .RuleFor(i => i.CurrentAttempt, 2)
                .RuleFor(i => i.AttemptsBeforeDeload, 2);

            _context.Exercises.Add(exercise);
            _context.SaveChanges();
            // Act          
            var request = new CompleteExerciseRequest()
            {
                Id = id,
                Reps = 7,
                Sets = 5
            };

            var response = await _client.PostAsJsonAsync(completeExerciseUrl
               , request);
            var result = _context.LinearProgressionExercises.Where(x => x.ParentId == id).FirstOrDefault();
            result.LiftWeek.Should().Be(2);
            result.WeightIndex.Should().Be(1);
            result.CurrentAttempt.Should().Be(2);
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            // Assert

        }
    }

    internal class ExerciseViewModel
    {
    }
}
