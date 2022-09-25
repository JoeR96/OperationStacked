using Microsoft.AspNetCore.Mvc;
using OperationStacked.Abstractions;
using OperationStacked.Requests;
using OperationStacked.Response;
using System.ComponentModel;
using System.Net.Mime;
using FluentResult;

namespace OperationStacked.Controllers
{
    [ApiController]
    [DisplayName("Workout Generation")]
    [Route("workout-creation/")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseCreationService _exerciseCreationService;
        private readonly IExerciseProgressionService _exerciseProgressionService;

        public ExerciseController(IExerciseCreationService workoutCreationService,
            IExerciseProgressionService exerciseProgressionService)
        {
            _exerciseCreationService = workoutCreationService;
            _exerciseProgressionService = exerciseProgressionService;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(WorkoutCreationResult))]
        [ProducesResponseType(400, Type = typeof(WorkoutCreationResult))]
        public async Task<IActionResult> GenerateWorkoutAsync([FromBody] CreateWorkoutRequest request)
        => await _exerciseCreationService.CreateWorkout(request)
            .ToActionResultAsync(this);

        [HttpPut]
        [ProducesResponseType(204, Type = typeof(ExerciseCompletionResult))]
        [ProducesResponseType(400, Type = typeof(ExerciseCompletionResult))]
        public Task<IActionResult> CompleteExerciseAsync([FromBody] CompleteExerciseRequest request)
        => _exerciseProgressionService.CompleteExercise(request)
            .ToActionResultAsync(this);

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(WorkoutCreationResult))]
        [ProducesResponseType(400, Type = typeof(WorkoutCreationResult))]
        public int Hello()
        => 1;
    }
}
