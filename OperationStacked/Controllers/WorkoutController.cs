using Microsoft.AspNetCore.Mvc;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStacked.Services.ExerciseCreationService;
using OperationStacked.Services.ExerciseProgressionService;
using OperationStacked.Services.ExerciseRetrievalService;
using System.ComponentModel;
using MoreLinq.Extensions;
using OperationStacked.Entities;
using OperationStacked.Models;
using CreateExerciseRequest = OperationStacked.Requests.CreateExerciseRequest;

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


        [HttpPost("CreateExercises")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Exercise>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExercisesAsync(
            [FromBody] List<CreateExerciseRequest> requests)
            => Ok(await _exerciseCreationService.CreateExercises(requests));


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
            => Ok(
                await _exerciseRetrievalService.GetWorkout(userId, week, day, completed));
        
        [HttpGet]
        [Route("{userId}/all")]
        [ProducesResponseType(200, Type = typeof(GetWorkoutResult))]
        public async Task<IActionResult> GetAllWorkouts(
            [FromRoute] Guid userId,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10)
        {
            // Pass pageIndex and pageSize to the service
            var result = await _exerciseRetrievalService.GetAllWorkouts(userId, pageIndex, pageSize);
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(result));
        }

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
        [ProducesResponseType(200,Type = typeof(decimal))]
        [Route("update")]
        public async Task<IActionResult> UpdateExerciseById(
            UpdateExerciseRequest request) => Ok(await _exerciseProgressionService.UpdateWorkingWeight(request));

        [HttpGet]
        [Route("GenerateDummy/{userId}")]
        public async Task<IActionResult> GenerateDummyData(
            [FromRoute] Guid userId)
        {
            var result = await _exerciseRetrievalService.GetAllWorkouts(userId, 0, 100);
            foreach (var exercise in result.Exercises)
            {
                // Assuming you have 15 iterations for each exercise.
                await CompleteExerciseRecursivelyAsync(15, exercise
                    .LinearProgressionExercises.FirstOrDefault());
            }

            // Assuming you want to return a success result after generating dummy data.
            return Ok("Dummy data generated successfully.");
        }

        private int

            GenerateExerciseOutcome(double failWeight, double passWeight, double progressWeight)
        {
            var random = new Random();
            var value = random.NextDouble();

            if (value < failWeight) return random.Next(1, 7);
            if (value < failWeight + passWeight) return random.Next(8, 12);
            return 12;
        }

        private async Task CompleteExerciseRecursivelyAsync(int count, LinearProgressionExercise exercise,
            double failWeight = 0.05,
            double passWeight = 0.4,
            double progressWeight = 0.4)
        {
            for (int i = 0; i < count; i++)
            {
                var outcomes = new List<int>();
                for (int sets = 0; sets < 3; sets++)
                {
                    var outcome = GenerateExerciseOutcome(failWeight, passWeight, progressWeight);
                    outcomes.Add(outcome);
                }

                var completeExerciseRequest = new CompleteExerciseRequest
                {
                    Reps = outcomes.ToArray(),
                    LinearProgressionExerciseId = exercise.WorkoutExercise.LinearProgressionExercises.Where(x => x.LiftWeek == i + 1 ).FirstOrDefault().Id,
                    Sets = outcomes.Count,
                    Template = ExerciseTemplate.LinearProgression,
                };

                await _exerciseProgressionService.CompleteExercise(completeExerciseRequest);
            }

        }
    }
}
