using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Enums;
using OperationStacked.Factories;
using OperationStacked.Models;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStacked.Services.WorkoutService;
using CreateExerciseRequest = OperationStacked.Requests.CreateExerciseRequest;

namespace OperationStacked.Services.ExerciseCreationService
{
    public class ExerciseCreationService : IExerciseCreationService
    {
        private readonly OperationStackedContext _operationStackedContext;
        private readonly LinearProgressionService _linearProgressionService;
        private readonly IWorkoutExerciseService _workoutExerciseService;
        private readonly IExerciseCreationService _exerciseCreationService;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IWorkoutService _workoutService;

        public ExerciseCreationService(
            OperationStackedContext operationStackedContext, LinearProgressionService linearProgressionService,
            IWorkoutExerciseService workoutExerciseService, IExerciseRepository exerciseRepository, IWorkoutService workoutService)
        {
            _operationStackedContext = operationStackedContext;
            _linearProgressionService = linearProgressionService;
            _workoutExerciseService = workoutExerciseService;
            _exerciseRepository = exerciseRepository;
            _workoutService = workoutService;
        }

        public async Task<List<Exercise>> CreateExercises(List<CreateExerciseRequest> requests)
        {
            return requests.Select(request => new Exercise
            {
                ExerciseName = request.ExerciseName,
                Category = request.Category,
                EquipmentType = request.EquipmentType,
                UserId = request.UserId
                // Id is not set here as it's assumed to be handled by the database context
            }).ToList();
        }

        public async Task<Exercise> CreateExercise(CreateExerciseRequest request)
        {
            return new Exercise
            {
                ExerciseName = request.ExerciseName,
                Category = request.Category,
                EquipmentType = request.EquipmentType,
                UserId = request.UserId
                // Id is not set here as it's assumed to be handled by the database context
            };
        }

      public async Task<WorkoutCreationResult> CreateWorkout(CreateWorkoutRequest request)
{
    try
    {
        List<LinearProgressionExercise> linearProgressionExercises = new List<LinearProgressionExercise>();
        List<Exercise> exercisesToInsert = new List<Exercise>();
        List<WorkoutExercise> workoutExercisesToInsert = new List<WorkoutExercise>();


        var workout = await _workoutService.CreateWorkout(request.WorkoutName,request.UserId);

        foreach (var lpRequest in request.Exercises)
        {
            // Determine if we need to create a new exercise or use an existing one.
            Exercise exercise = await CreateExercise(lpRequest.WorkoutExercise.Exercise);
            exercisesToInsert.Add(exercise);
        }

        // Add and save Exercise entities first to ensure they have IDs generated.
        _operationStackedContext.Exercises.AddRange(exercisesToInsert);
        await _operationStackedContext.SaveChangesAsync();

        foreach (var lpRequest in request.Exercises)
        {
            // Get the exercise with the generated ID
            Exercise exercise = exercisesToInsert.First(e => e.ExerciseName == lpRequest.WorkoutExercise.Exercise.ExerciseName && e.UserId == lpRequest.WorkoutExercise.Exercise.UserId && e.Category == lpRequest.WorkoutExercise.Exercise.Category); // Make sure this comparison is adequate for your scenario

            // Create the WorkoutExercise with the correct ExerciseId.

            Guid equipmentStackId;
            WorkoutExercise workoutExercise = await _workoutExerciseService.CreateWorkoutExercise(
                new CreateWorkoutExerciseRequest()
                {
                    ExerciseId = exercise.Id,
                    Template = lpRequest.WorkoutExercise.Template,
                    LiftDay = lpRequest.WorkoutExercise.LiftDay,
                    LiftOrder = lpRequest.WorkoutExercise.LiftOrder,
                    RestTimer = lpRequest.WorkoutExercise.RestTimer,
                    WorkoutId = workout.Id,

                });

            if (exercise.EquipmentType is EquipmentType.Cable or EquipmentType.Machine)
            {
                var response = await _exerciseRepository.InsertEquipmentStack(lpRequest.EquipmentStack);
                workoutExercise.EquipmentStackId = response.Stack.Id;
            }

            workoutExercisesToInsert.Add(workoutExercise);
            _operationStackedContext.WorkoutExercises.AddRange(workoutExercisesToInsert);

            var linearProgressionExercise = await _linearProgressionService.CreateLinearProgressionExercise(lpRequest,
                 workoutExercise);


            linearProgressionExercises.Add(linearProgressionExercise);
        }
        _operationStackedContext.WorkoutExercises.AddRange(workoutExercisesToInsert);

        await _operationStackedContext.SaveChangesAsync();

        _operationStackedContext.LinearProgressionExercises.AddRange(linearProgressionExercises);


        await _operationStackedContext.SaveChangesAsync();

        return new WorkoutCreationResult(
            linearProgressionExercises.Any() ? WorkoutCreatedStatus.Created : WorkoutCreatedStatus.Error,
            linearProgressionExercises);
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException("An error occurred while creating the workout.", ex);
    }
}

    }


}
