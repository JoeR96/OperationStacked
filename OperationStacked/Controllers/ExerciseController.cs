using Microsoft.AspNetCore.Mvc;
using OperationStacked.Abstractions;
using OperationStacked.Requests;
using OperationStacked.Response;
using System.ComponentModel;
using System.Net.Mime;
using FluentResult;
using Bogus;
using OperationStacked.Models;

namespace OperationStacked.Controllers
{
    [ApiController]
    [DisplayName("Workout Generation")]
    [Route("workout-creation/")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseCreationService _exerciseCreationService;
        private readonly IExerciseProgressionService _exerciseProgressionService;
        private readonly IExerciseRetrievalService _exerciseRetrievalService;

        public ExerciseController(IExerciseCreationService workoutCreationService,
            IExerciseProgressionService exerciseProgressionService,
            IExerciseRetrievalService? exerciseRetrievalService)
        {
            _exerciseCreationService = workoutCreationService;
            _exerciseProgressionService = exerciseProgressionService;
            _exerciseRetrievalService = exerciseRetrievalService;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(WorkoutCreationResult))]
        [ProducesResponseType(400, Type = typeof(WorkoutCreationResult))]
        public async Task<IActionResult> GenerateWorkoutAsync([FromBody] CreateWorkoutRequest request)
        => await _exerciseCreationService.CreateWorkout(request)
            .ToActionResultAsync(this);
        [Route("complete")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ExerciseCompletionResult))]
        [ProducesResponseType(400, Type = typeof(ExerciseCompletionResult))]
        public Task<IActionResult> CompleteExerciseAsync([FromBody] CompleteExerciseRequest request)
        => _exerciseProgressionService.CompleteExercise(request)
            .ToActionResultAsync(this);

        [HttpGet]
        [Route("{userId}/{week}/{day}")]
        [ProducesResponseType(200, Type = typeof(WorkoutCreationResult))]
        [ProducesResponseType(400, Type = typeof(WorkoutCreationResult))]
        public Task<IActionResult> GetWorkout([FromRoute] int userId, [FromRoute] int week, [FromRoute] int day)
            => _exerciseRetrievalService.GetWorkout(userId, week, day)
                .ToActionResultAsync(this);

        [HttpGet]
        [Route("dummydata")]
        public Task<IActionResult> Dummydata(int userId)
        {
            var exercises = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Legs")
                .RuleFor(x => x.ExerciseName, "Squats")
                .RuleFor(x => x.Template, "LinearProgression")
                .RuleFor(x => x.Username, "ChickenLegTim")
                .RuleFor(x => x.LiftDay, 1)
                .RuleFor(x => x.LiftOrder, 1)
                .RuleFor(x => x.UserId, 1)
                .Generate(5);

            var request = new CreateWorkoutRequest();
            request.ExerciseDaysAndOrders = exercises;
            request.userId = userId;
            return _exerciseCreationService.CreateWorkout(request).ToActionResultAsync(this);
        }
    }
}
