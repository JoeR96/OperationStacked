using Bogus;
using FluentAssertions;
using NUnit.Framework;
using OperationStacked.Entities;
using OperationStacked.Enums;
using OperationStacked.Models;
using OperationStacked.Requests;
using System.Net.Http.Json;

namespace OperationStackedTests.Functional
{
    [TestFixture]
    public class LinearProgressionProgressTests : BaseApiTest
    {
        private const string completeExerciseUrl = "/workout-creation/complete";

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
                Reps = new int[] { 12, 12, 12 },
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
                            .RuleFor(i => i.WeightIndex, 1)
                            .RuleFor(i => i.EquipmentType, EquipmentType.Barbell)
                .RuleFor(i => i.Template, ExerciseTemplate.LinearProgression);
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
                .RuleFor(i => i.AttemptsBeforeDeload, 2)
                .RuleFor(i => i.EquipmentType, EquipmentType.Barbell)
                .RuleFor(i => i.Template, ExerciseTemplate.LinearProgression);

            _context.Exercises.Add(exercise);
            _context.SaveChanges();
            // Act          
            var request = new CompleteExerciseRequest()
            {
                Id = id,
                Reps = new int[] { 12, 12, 12 },
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
                .RuleFor(i => i.AttemptsBeforeDeload, 2)
                .RuleFor(i => i.EquipmentType, EquipmentType.Barbell)
                .RuleFor(i => i.Template, ExerciseTemplate.LinearProgression);

            _context.Exercises.Add(exercise);
            _context.SaveChanges();
            // Act          
            var request = new CompleteExerciseRequest()
            {
                Id = id,
                Reps = new int[] { 7, 8, 8 },
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
                .RuleFor(i => i.AttemptsBeforeDeload, 2)
                .RuleFor(i => i.EquipmentType, EquipmentType.Barbell)
                .RuleFor(i => i.Template, ExerciseTemplate.LinearProgression);

            _context.Exercises.Add(exercise);
            _context.SaveChanges();
            // Act          
            var request = new CompleteExerciseRequest()
            {
                Id = id,
                Reps = new int[] { 7, 8, 8 },
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
                .RuleFor(i => i.AttemptsBeforeDeload, 2)
                .RuleFor(i => i.EquipmentType, EquipmentType.Barbell)
                .RuleFor(i => i.Template, ExerciseTemplate.LinearProgression);

            _context.Exercises.Add(exercise);
            _context.SaveChanges();
            // Act          
            var request = new CompleteExerciseRequest()
            {
                Id = id,
                Reps = new int[] { 7, 8, 8 },
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
}
