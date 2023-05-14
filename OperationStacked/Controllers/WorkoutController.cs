using Microsoft.AspNetCore.Mvc;
using OperationStacked.Requests;
using OperationStacked.Response;
using System.ComponentModel;
using System.Net.Mime;
using FluentResult;
using Bogus;
using OperationStacked.Models;
using OperationStacked.Services.ExerciseProgressionService;
using OperationStacked.Services.ExerciseCreationService;
using OperationStacked.Entities;
using OperationStacked.Services.ExerciseRetrievalService;
using Microsoft.AspNetCore.Authorization;

namespace OperationStacked.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> GenerateWorkoutAsync(
            [FromBody] CreateWorkoutRequest request)
        => await _exerciseCreationService.CreateWorkout(request)
            .ToActionResultAsync(this);
        [Route("complete")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ExerciseCompletionResult))]
        [ProducesResponseType(400, Type = typeof(ExerciseCompletionResult))]
        public Task<IActionResult> CompleteExerciseAsync(
            [FromBody] CompleteExerciseRequest request)
        => _exerciseProgressionService.CompleteExercise(request)
            .ToActionResultAsync(this);

        [HttpGet]
        [Route("{userId}/{week}/{day}/{completed}")]
        [ProducesResponseType(200, Type = typeof(GetWorkoutResult))]
        [ProducesResponseType(400, Type = typeof(GetWorkoutResult))]
        public async Task<IActionResult> GetWorkout(
            [FromRoute] string userId, 
            [FromRoute] int week,
            [FromRoute] int day, 
            [FromRoute] bool completed)
            => Ok(Newtonsoft.Json.JsonConvert.SerializeObject(
                await _exerciseRetrievalService.GetWorkout(userId, week, day,completed)));

        [HttpGet]
        [Route("dummydata")]
        public Task<IActionResult> Dummydata(string userId)
        {

            
            var request = new CreateWorkoutRequest();
            var LegsOne = CreateLegsOne(userId, "Legs", 1);
            var PushOne = CreatePushOne(userId);
            var PullOne = CreatePullOne(userId, "Back", 3);
            var PushTwo = CreatePushTwo(userId);
            var PullLegs = CreatePullLegs(userId, "Legs/Back", 5);

            var gigalist = LegsOne.Concat(PushOne).Concat(PullOne).Concat(PushTwo).Concat(PullLegs).ToList();
            request.ExerciseDaysAndOrders = gigalist;
            request.userId = userId;
            return _exerciseCreationService
                .CreateWorkout(request)
                .ToActionResultAsync(this);
        }

        private List<CreateExerciseModel> CreateLegsOne(string userId, string v1, int v2)
        {
            var Squats = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Legs")
                .RuleFor(x => x.ExerciseName, "Back Squat")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Barbell)
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.LiftDay, 1)
                .RuleFor(x => x.LiftOrder, 2)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 40)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 10)
                .RuleFor(x => x.WeightProgression, 5);

            var adductor = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Legs")
                .RuleFor(x => x.ExerciseName, "Romanian Deadlift")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Dumbbell)
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.LiftDay, 1)
                .RuleFor(x => x.LiftOrder, 2)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 28)
                .RuleFor(x => x.WeightIndex, 14)
                .RuleFor(x => x.TargetSets, 4)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 10);

            var abd = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Legs")
                .RuleFor(x => x.ExerciseName, "Leg Extensions")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Machine)
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.LiftDay, 1)
                .RuleFor(x => x.LiftOrder, 3)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 35)
                .RuleFor(x => x.WeightIndex, 5)
                .RuleFor(x => x.TargetSets, 4)
                .RuleFor(x => x.MinimumReps, 10)
                .RuleFor(x => x.MaximumReps, 12);

            var adduc = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Legs")
                .RuleFor(x => x.ExerciseName, "Leg Curls")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Machine)
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.LiftDay, 1)
                .RuleFor(x => x.LiftOrder, 4)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 28)
                .RuleFor(x => x.WeightIndex, 35)
                .RuleFor(x => x.TargetSets, 5)
                .RuleFor(x => x.MinimumReps, 10)
                .RuleFor(x => x.MaximumReps, 12);

            return new List<CreateExerciseModel> { Squats, adductor, abd, adduc};
        }

        private static List<CreateExerciseModel> CreatePushOne(string userId)
        {
            var ohp = new Faker<CreateExerciseModel>()
                            .RuleFor(x => x.Category, "Shoulders")
                            .RuleFor(x => x.ExerciseName, "Overhead Press")
                              .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                                .RuleFor(x => x.Username, "JoeR96")
                                .RuleFor(x => x.LiftDay, 2)
                                .RuleFor(x => x.LiftOrder, 1)
                                .RuleFor(x => x.UserId, userId)
                                .RuleFor(x => x.StartingWeight, 18)
                                .RuleFor(x => x.WeightIndex, 9)
                                .RuleFor(x => x.TargetSets, 4)
                                .RuleFor(x => x.MinimumReps, 8)
                                .RuleFor(x => x.MaximumReps, 12)
                                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Dumbbell);
            
            var ulpd = new Faker<CreateExerciseModel>()
 .RuleFor(x => x.Category, "Back")
                .RuleFor(x => x.ExerciseName, "Unilateral pull down")
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Cable)
                .RuleFor(x => x.LiftDay, 2)
                .RuleFor(x => x.LiftOrder, 2)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 50)
                .RuleFor(x => x.WeightIndex, 0)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 12)
                .RuleFor(x => x.WeightProgression, 1.25m);

            var chestFly = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Chest")
                .RuleFor(x => x.ExerciseName, "Pec Dec")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Machine)
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.LiftDay, 2)
                .RuleFor(x => x.LiftOrder, 3)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 59)
                .RuleFor(x => x.WeightIndex, 9)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 12);

            var inclineSmithPress = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Shoulders")
                .RuleFor(x => x.ExerciseName, "Lateral Raises")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Dumbbell)
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.LiftDay, 2)
                .RuleFor(x => x.LiftOrder, 3)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 6)
                .RuleFor(x => x.WeightIndex, 4)
                .RuleFor(x => x.TargetSets, 4)
                .RuleFor(x => x.MinimumReps, 10)
                .RuleFor(x => x.MaximumReps, 12);

            var dumbellCurls = new Faker<CreateExerciseModel>()
               .RuleFor(x => x.Category, "Biceps")
               .RuleFor(x => x.ExerciseName, "Cable Curl 1-2-3-4 BURN")
               .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
               .RuleFor(x => x.Username, "JoeR96")
               .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Cable)
               .RuleFor(x => x.LiftDay, 2)
               .RuleFor(x => x.LiftOrder, 5)
               .RuleFor(x => x.UserId, userId)
               .RuleFor(x => x.StartingWeight, 10)
               .RuleFor(x => x.WeightIndex, 5)
               .RuleFor(x => x.TargetSets, 3)
               .RuleFor(x => x.MinimumReps, 10)
               .RuleFor(x => x.MaximumReps, 12);

  
            var tricepRopePushDown = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Triceps")
                .RuleFor(x => x.ExerciseName, "Tricep Rope Pushdown")
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Cable)
                .RuleFor(x => x.LiftDay, 2)
                .RuleFor(x => x.LiftOrder, 6)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 18m)
                .RuleFor(x => x.WeightIndex, 5)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 12)
                .RuleFor(x => x.MaximumReps, 15);

            return new List<CreateExerciseModel> { ohp, ulpd, chestFly, inclineSmithPress, dumbellCurls,tricepRopePushDown };
        }

        private List<CreateExerciseModel> CreatePullOne(string id, string category, int day )
        {
            var deadlift = new Faker<CreateExerciseModel>()
                    .RuleFor(x => x.Category, category)
                    .RuleFor(x => x.ExerciseName, "Deadlift")
                    .RuleFor(x => x.PrimaryLift, true)
                    .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                    .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Barbell)
                    .RuleFor(x => x.Username, "JoeR96")
                    .RuleFor(x => x.LiftDay, day)
                    .RuleFor(x => x.LiftOrder, 1)
                    .RuleFor(x => x.UserId, id)
                    .RuleFor(x => x.StartingWeight, 100)
                    .RuleFor(x => x.WeightIndex, 0)
                    .RuleFor(x => x.TargetSets, 3)
                    .RuleFor(x => x.MinimumReps, 5)
                    .RuleFor(x => x.MaximumReps, 5)
                    .RuleFor(x => x.WeightProgression, 5m);

            var unilateralPulldown = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, category)
                .RuleFor(x => x.ExerciseName, "Lunge")
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Dumbbell)
                .RuleFor(x => x.LiftDay, day)
                .RuleFor(x => x.LiftOrder, 2)
                .RuleFor(x => x.UserId, id)
                .RuleFor(x => x.StartingWeight, 10)
                .RuleFor(x => x.WeightIndex, 5)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 10);


            var lowRow = new Faker<CreateExerciseModel>()
               .RuleFor(x => x.Category, category)
               .RuleFor(x => x.ExerciseName, "Leg Press (Wide Stance)")
               .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
               .RuleFor(x => x.Username, "JoeR96")
               .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Machine)
               .RuleFor(x => x.LiftDay, day)
               .RuleFor(x => x.LiftOrder, 2)
                .RuleFor(x => x.UserId, id)
                .RuleFor(x => x.StartingWeight, 40)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 10)
                .RuleFor(x => x.WeightProgression, 5m);

            var assistedPullups = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, category)
                .RuleFor(x => x.ExerciseName, "Hamstring Curls")
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Machine)
                .RuleFor(x => x.LiftDay, day)
                .RuleFor(x => x.LiftOrder, 4)
                .RuleFor(x => x.UserId, id)
                .RuleFor(x => x.StartingWeight, 18m)
                .RuleFor(x => x.WeightIndex, 5)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 12)
                .RuleFor(x => x.MaximumReps, 15);


            return new List<CreateExerciseModel> { deadlift, unilateralPulldown, assistedPullups };
        }
        private static List<CreateExerciseModel> CreatePushTwo(string userId)
        {
            var ohp = new Faker<CreateExerciseModel>()
                            .RuleFor(x => x.Category, "Chest")
                            .RuleFor(x => x.ExerciseName, "Incline Dumbell Press")
                            .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                            .RuleFor(x => x.Username, "JoeR96")
                            .RuleFor(x => x.LiftDay, 4)
                            .RuleFor(x => x.LiftOrder, 1)
                            .RuleFor(x => x.UserId, userId)
                            .RuleFor(x => x.StartingWeight, 20)
                            .RuleFor(x => x.WeightIndex, 10)
                            .RuleFor(x => x.TargetSets, 3)
                            .RuleFor(x => x.MinimumReps, 8)
                            .RuleFor(x => x.MaximumReps, 12);

            var chestFly = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Back")
                .RuleFor(x => x.ExerciseName, "Closegrip Lateral Pulldown")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Machine)
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.LiftDay, 4)
                .RuleFor(x => x.LiftOrder, 2)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 35)
                .RuleFor(x => x.WeightIndex, 6)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 12);

            var inclineSmithPress = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Chest")
                .RuleFor(x => x.ExerciseName, "Incline Shoulder Press")
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.SmithMachine)
                .RuleFor(x => x.LiftDay, 4)
                .RuleFor(x => x.LiftOrder, 3)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 35)
                .RuleFor(x => x.WeightIndex, 6)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 12);
            var bbc = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Biceps")
                .RuleFor(x => x.ExerciseName, "Incline Dumbell Curls")
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Dumbbell)
                .RuleFor(x => x.LiftDay, 4)
                .RuleFor(x => x.LiftOrder, 5)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 12)
                .RuleFor(x => x.WeightIndex, 6)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 12);

            var dumbBellLatRaise = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Shoulders")
                .RuleFor(x => x.ExerciseName, "Lateral Raise")
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Dumbbell)
                .RuleFor(x => x.LiftDay, 4)
                .RuleFor(x => x.LiftOrder, 5)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 8)
                .RuleFor(x => x.WeightIndex, 7)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 12);

            var tricepRopePushDown = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Triceps")
                .RuleFor(x => x.ExerciseName, "Tricep Rope Pushdown")
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Cable)
                .RuleFor(x => x.LiftDay, 4)
                .RuleFor(x => x.LiftOrder, 5)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 18m)
                .RuleFor(x => x.WeightIndex, 6)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 8)
                .RuleFor(x => x.MaximumReps, 12);

            return new List<CreateExerciseModel> { ohp, chestFly, inclineSmithPress, dumbBellLatRaise, tricepRopePushDown };
        }
        private List<CreateExerciseModel> CreatePullLegs(string userId, string v1, int v2)
        {
            var frontSquat = new Faker<CreateExerciseModel>()
                            .RuleFor(x => x.Category, "Legs")
                            .RuleFor(x => x.ExerciseName, "Front Squat")
                            .RuleFor(x => x.Template, ExerciseTemplate.A2SHypertrophy)
                            .RuleFor(x => x.PrimaryLift, true)
                            .RuleFor(x => x.Username, "JoeR96")
                            .RuleFor(x => x.LiftDay, 5)
                            .RuleFor(x => x.LiftOrder, 1)
                            .RuleFor(x => x.UserId, userId)
                            .RuleFor(x => x.TrainingMax, 87.5m)
                            .RuleFor(x => x.WeightIndex, 0)
                            .RuleFor(x => x.Block, Enums.A2SBlocks.Hypertrophy)
                            .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Barbell)
                            .RuleFor(x => x.WeightProgression, 2.5m);

            var romanianDeadlift = new Faker<CreateExerciseModel>()
                            .RuleFor(x => x.Category, "Legs")
                            .RuleFor(x => x.ExerciseName, "Romanian Deadlift")
                            .RuleFor(x => x.Template, ExerciseTemplate.A2SHypertrophy)
                            .RuleFor(x => x.PrimaryLift, false)
                            .RuleFor(x => x.Username, "JoeR96")
                            .RuleFor(x => x.LiftDay, 5)
                            .RuleFor(x => x.LiftOrder, 2)
                            .RuleFor(x => x.UserId, userId)
                            .RuleFor(x => x.TrainingMax, 120)
                            .RuleFor(x => x.WeightIndex, 0)
                            .RuleFor(x => x.Block, Enums.A2SBlocks.Hypertrophy)
                            .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Barbell)
                            .RuleFor(x => x.WeightProgression, 5m);

            var abductor = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Shoulders")
                .RuleFor(x => x.ExerciseName, "Overhead Press")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Barbell)
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.LiftDay, 5)
                .RuleFor(x => x.LiftOrder, 3)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 30)
                .RuleFor(x => x.WeightIndex, 9)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 12)
                .RuleFor(x => x.MaximumReps, 15);

            var legCurl = new Faker<CreateExerciseModel>()
                .RuleFor(x => x.Category, "Back")
                .RuleFor(x => x.ExerciseName, "Low Row")
                .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Machine)
                .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                .RuleFor(x => x.Username, "JoeR96")
                .RuleFor(x => x.LiftDay, 5)
                .RuleFor(x => x.LiftOrder, 4)
                .RuleFor(x => x.UserId, userId)
                .RuleFor(x => x.StartingWeight, 59)
                .RuleFor(x => x.WeightIndex, 9)
                .RuleFor(x => x.TargetSets, 3)
                .RuleFor(x => x.MinimumReps, 12)
                .RuleFor(x => x.MaximumReps, 15);

            var legExtension = new Faker<CreateExerciseModel>()
                 .RuleFor(x => x.Category, "Big Dave Arms")
                 .RuleFor(x => x.ExerciseName, "Bicep Curls")
                 .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Dumbbell)
                 .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                 .RuleFor(x => x.Username, "JoeR96")
                 .RuleFor(x => x.LiftDay, 5)
                 .RuleFor(x => x.LiftOrder, 5)
                 .RuleFor(x => x.UserId, userId)
                 .RuleFor(x => x.StartingWeight, 10)
                 .RuleFor(x => x.WeightIndex, 9)
                 .RuleFor(x => x.TargetSets, 3)
                 .RuleFor(x => x.MinimumReps, 12)
                 .RuleFor(x => x.MaximumReps, 15);

            var legExtensions = new Faker<CreateExerciseModel>()
                 .RuleFor(x => x.Category, "Big Dave Arms")
                 .RuleFor(x => x.ExerciseName, "Tricep Pushdown")
                 .RuleFor(x => x.EquipmentType, Enums.EquipmentType.Dumbbell)
                 .RuleFor(x => x.Template, ExerciseTemplate.LinearProgression)
                 .RuleFor(x => x.Username, "JoeR96")
                 .RuleFor(x => x.LiftDay, 5)
                 .RuleFor(x => x.LiftOrder, 6)
                 .RuleFor(x => x.UserId, userId)
                 .RuleFor(x => x.StartingWeight, 10)
                 .RuleFor(x => x.WeightIndex, 9)
                 .RuleFor(x => x.TargetSets, 3)
                 .RuleFor(x => x.MinimumReps, 12)
                 .RuleFor(x => x.MaximumReps, 15);

            return new List<CreateExerciseModel> { frontSquat, romanianDeadlift, abductor, legCurl, legExtension, legExtensions };
        }
    }
}
