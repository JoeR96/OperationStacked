using Microsoft.AspNetCore.Mvc;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStacked.Services.ExerciseCreationService;
using OperationStacked.Services.ExerciseProgressionService;
using OperationStacked.Services.ExerciseRetrievalService;
using System.ComponentModel;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Repositories.WorkoutRepository;

namespace OperationStacked.Controllers
{
    [ApiController]
    [DisplayName("Workout Generation")]
    [Route("workout/")]
    public class WorkoutController : ControllerBase
    {
        private readonly IExerciseCreationService _exerciseCreationService;
        private readonly IWorkoutExerciseProgressionService _workoutExerciseProgressionService;
        private readonly IExerciseRetrievalService _exerciseRetrievalService;
        private readonly IExerciseRepository _exerciseRepository;
        private IWorkoutRepository _workoutRepository;

        public WorkoutController(IExerciseCreationService workoutCreationService,
            IWorkoutExerciseProgressionService workoutExerciseProgressionService,
            IExerciseRetrievalService exerciseRetrievalService, 
            IExerciseRepository exerciseRepository, IWorkoutRepository workoutRepository)
        {
            _exerciseCreationService = workoutCreationService;
            _workoutExerciseProgressionService = workoutExerciseProgressionService;
            _exerciseRetrievalService = exerciseRetrievalService;
            _exerciseRepository = exerciseRepository;
            _workoutRepository = workoutRepository;
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
            => Ok(await _workoutExerciseProgressionService.CompleteExercise(request));

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
            var result = await _exerciseRetrievalService.GetAllWorkouts(userId, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet]
        [Route("GenerateDummy/")]
        public async Task<IActionResult> GenerateDummyData()
        {
            var result = await _exerciseRetrievalService.GetAllWorkouts(Guid.Parse("5af5dae7-801e-47c0-bfc9-3eac5b25491c"), 0, 100);

                foreach (var exercise in result.Exercises)
                {
                    await CompleteExerciseAsync(15, exercise);
                }

            return Ok("Dummy data generated successfully.");
        }

        private static int  GenerateExerciseOutcome(double failWeight, double passWeight, double progressWeight)
        {
            var random = new Random();
            var value = random.NextDouble();

            if (value < failWeight) return random.Next(1, 7);
            if (value < failWeight + passWeight) return random.Next(8, 12);
            return 12;
        }

        private async Task CompleteExerciseAsync(int count, WorkoutExercise exercise,
            double failWeight = 0.05,
            double passWeight = 0.4,
            double progressWeight = 0.4)
        {
            for (int i = 0; i < count ; i++)
            {
                var outcomes = new List<int>();
                for (int sets = 0; sets < 3; sets++)
                {
                    var outcome = GenerateExerciseOutcome(failWeight, passWeight, progressWeight);
                    outcomes.Add(outcome);
                }

                exercise = await _exerciseRepository.GetWorkoutExerciseById(exercise.Id);
                var exerciseForWeek = exercise.LinearProgressionExercises?.Where(x => x.LiftWeek == i + 1).FirstOrDefault();
                if (exerciseForWeek != null)
                {
                    var completeExerciseRequest = new CompleteExerciseRequest
                    {
                        Reps = outcomes.ToArray(),
                        LinearProgressionExerciseId = exerciseForWeek.Id,
                        Sets = outcomes.Count,
                        Template = ExerciseTemplate.LinearProgression,
                        ExerciseId = exerciseForWeek.WorkoutExercise.ExerciseId
                    };

                    await _workoutExerciseProgressionService.CompleteExercise(completeExerciseRequest);
                }
                else
                {
                    // Handle the case where there's no exercise scheduled for the current week.
                }
            }
        }

    }
}
