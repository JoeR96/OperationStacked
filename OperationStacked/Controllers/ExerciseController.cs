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
            var ohp = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Shoulders")
                .RuleFor(x => x.ExerciseName, "Overhead Press")
                .RuleFor(x => x.Template, "LinearProgression")
                .RuleFor(x => x.Username, "BigDaveTV")
                .RuleFor(x => x.LiftDay, 1)
                .RuleFor(x => x.LiftOrder, 1)
                .RuleFor(x => x.UserId, 1)
                .RuleFor(x => x.StartingWeight, 40)
                .RuleFor(x => x.WeightIndex, 0)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 12)
                .RuleFor(x => x.EquipmentType, "Barbell")
                .RuleFor(x => x.WeightProgression, 1.25m);

            var chestFly = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Chest")
                .RuleFor(x => x.ExerciseName, "Pec Dec")
                .RuleFor(x => x.EquipmentType, "Machine")
                .RuleFor(x => x.Template, "LinearProgression")
                .RuleFor(x => x.Username, "BigDaveTV")
                .RuleFor(x => x.LiftDay, 1)
                .RuleFor(x => x.LiftOrder, 2)
                .RuleFor(x => x.UserId, 1)
                .RuleFor(x => x.StartingWeight, 59)
                .RuleFor(x => x.WeightIndex, 9)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 12);
            var inclineSmithPress = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Chest")
                .RuleFor(x => x.ExerciseName, "Incline Smith Press")
                .RuleFor(x => x.Template, "LinearProgression")
                .RuleFor(x => x.Username, "BigDaveTV")
                .RuleFor(x => x.EquipmentType, "Smith Machine")
                .RuleFor(x => x.LiftDay, 1)
                .RuleFor(x => x.LiftOrder, 3)
                .RuleFor(x => x.UserId, 1)
                .RuleFor(x => x.StartingWeight, 50)
                .RuleFor(x => x.WeightIndex, 0)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 12)
                .RuleFor(x => x.WeightProgression, 1.25m);


            var dumbBellLatRaise = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Shoulders")
                .RuleFor(x => x.ExerciseName, "Lateral Raise")
                .RuleFor(x => x.Template, "LinearProgression")
                .RuleFor(x => x.Username, "BigDaveTV")
                .RuleFor(x => x.EquipmentType, "Dumbbell")
                .RuleFor(x => x.LiftDay, 1)
                .RuleFor(x => x.LiftOrder, 4)
                .RuleFor(x => x.UserId, 1)
                .RuleFor(x => x.StartingWeight, 8)
                .RuleFor(x => x.WeightIndex, 7)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 12);

            var tricepRopePushDown = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Triceps")
                .RuleFor(x => x.ExerciseName, "Tricep Rope Pushdown")
                .RuleFor(x => x.Template, "LinearProgression")
                .RuleFor(x => x.Username, "BigDaveTV")
                .RuleFor(x => x.EquipmentType, "Cable")
                .RuleFor(x => x.LiftDay, 1)
                .RuleFor(x => x.LiftOrder, 5)
                .RuleFor(x => x.UserId, 1)
                .RuleFor(x => x.StartingWeight, 18m)
                .RuleFor(x => x.WeightIndex, 5)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 12);

            var request = new CreateWorkoutRequest();
            request.ExerciseDaysAndOrders = new List<CreateExerciseModel> { ohp,chestFly,inclineSmithPress,dumbBellLatRaise,tricepRopePushDown};
            request.userId = userId;
            return _exerciseCreationService.CreateWorkout(request).ToActionResultAsync(this);
        }
    }
}
