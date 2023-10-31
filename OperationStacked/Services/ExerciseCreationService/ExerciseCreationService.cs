using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Factories;
using OperationStacked.Models;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using OperationStacked.Response;
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

        public ExerciseCreationService(
            OperationStackedContext operationStackedContext, LinearProgressionService linearProgressionService,
            IWorkoutExerciseService workoutExerciseService, IExerciseRepository exerciseRepository)
        {
            _operationStackedContext = operationStackedContext;
            _linearProgressionService = linearProgressionService;
            _workoutExerciseService = workoutExerciseService;
            _exerciseRepository = exerciseRepository;
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
            WorkoutExercise workoutExercise = await _workoutExerciseService.CreateWorkoutExercise(
                new CreateWorkoutExerciseRequest()
                {
                    ExerciseId = exercise.Id,
                    Template = lpRequest.WorkoutExercise.Template,
                    LiftDay = lpRequest.WorkoutExercise.LiftDay,
                    LiftOrder = lpRequest.WorkoutExercise.LiftOrder,
                    RestTimer = lpRequest.WorkoutExercise.RestTimer,
                });
            workoutExercisesToInsert.Add(workoutExercise);
            _operationStackedContext.WorkoutExercises.AddRange(workoutExercisesToInsert);

            var linearProgressionExercise = await _linearProgressionService.CreateLinearProgressionExercise(
                lpRequest, workoutExercise, request.UserId);


            linearProgressionExercises.Add(linearProgressionExercise);
        }
        _operationStackedContext.WorkoutExercises.AddRange(workoutExercisesToInsert);

        await _operationStackedContext.SaveChangesAsync();

        // Add WorkoutExercise entities to the context.
        _operationStackedContext.LinearProgressionExercises.AddRange(linearProgressionExercises);

        // If LinearProgressionExercises have a dependency on WorkoutExercises, they should be added here as well.

        // Save all the remaining changes in one transaction.
        await _operationStackedContext.SaveChangesAsync();

        return new WorkoutCreationResult(
            linearProgressionExercises.Any() ? WorkoutCreatedStatus.Created : WorkoutCreatedStatus.Error,
            linearProgressionExercises);
    }
    catch (Exception ex)
    {
        // Log the exception, wrap it in a more user-friendly message, or handle it as needed.
        throw new InvalidOperationException("An error occurred while creating the workout.", ex);
    }
}

    }
}
