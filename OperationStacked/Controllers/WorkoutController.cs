using Microsoft.AspNetCore.Mvc;
using OperationStacked.Requests;
using OperationStacked.Response;
using System.ComponentModel;
using OperationStacked.Entities;
using OperationStacked.Repositories;
using OperationStacked.Services.ExerciseProgressionService;
using OperationStacked.Services.ExerciseCreationService;
using OperationStacked.Services.ExerciseRetrievalService;

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
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseController(IExerciseCreationService workoutCreationService,
            IExerciseProgressionService exerciseProgressionService,
            IExerciseRetrievalService exerciseRetrievalService, 
            IExerciseRepository exerciseRepository)
        {
            _exerciseCreationService = workoutCreationService;
            _exerciseProgressionService = exerciseProgressionService;
            _exerciseRetrievalService = exerciseRetrievalService;
            _exerciseRepository = exerciseRepository;
        }

        
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(WorkoutCreationResult))]
        public async Task<IActionResult> GenerateWorkoutAsync(
            [FromBody] CreateWorkoutRequest request)
            => Ok(await _exerciseCreationService.CreateWorkout(request));
    
        [Route("complete")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ExerciseCompletionResult))]
        public async Task<IActionResult> CompleteExerciseAsync(
            [FromBody] CompleteExerciseRequest request)
            => Ok(await _exerciseProgressionService.CompleteExercise(request));

        [HttpGet]
        [Route("{userId}/{week}/{day}/{completed}")]
        [ProducesResponseType(200, Type = typeof(GetWorkoutResult))]
        public async Task<IActionResult> GetWorkout(
            [FromRoute] Guid userId,
            [FromRoute] int week,
            [FromRoute] int day,
            [FromRoute] bool completed)
            => Ok(Newtonsoft.Json.JsonConvert.SerializeObject(
                await _exerciseRetrievalService.GetWorkout(userId, week, day, completed)));

        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Route("{exerciseId}/delete")]
        public async Task<IActionResult> DeleteExercise(
            [FromRoute] Guid exerciseId) => Ok(await _exerciseRepository.DeleteExercise(exerciseId));
        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(bool))]
        [Route("{userId}/delete-all")]
        public async Task<IActionResult> DeleteAllExercisesForUser(
            [FromRoute] Guid userId) => Ok(await _exerciseRepository.DeleteAllExercisesForUser(userId));

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(GetExerciseResult))]
        [Route("{exerciseId}")]
        public async Task<IActionResult> GetExerciseById(
            [FromRoute] Guid exerciseId) => Ok(await _exerciseRepository.GetExerciseById(exerciseId));
        
        [HttpPut]
        [ProducesResponseType(200,Type = typeof(bool))]
        [Route("{exerciseId}/{weight}")]
        public async Task<IActionResult> UpdateExerciseById(
            [FromRoute] Guid exerciseId,
            [FromRoute] decimal weight) => Ok(await _exerciseRepository.UpdateExerciseById(exerciseId,weight));
    }
}
