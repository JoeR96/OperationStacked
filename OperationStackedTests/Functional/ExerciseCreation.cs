using Bogus;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using OperationStacked.Models;
using OperationStacked.Requests;
using System.Net.Http.Json;

namespace OperationStackedTests.Functional
{
    [TestFixture]
    public class ExerciseCreation : BaseApiTest
    {
        private const string url = "/workout-creation";


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

       
    }

    internal class ExerciseViewModel
    {
    }
}
