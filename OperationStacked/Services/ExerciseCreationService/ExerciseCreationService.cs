using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Factories;
using OperationStacked.Models;
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

        public ExerciseCreationService(
            OperationStackedContext operationStackedContext, LinearProgressionService linearProgressionService,
            IWorkoutExerciseService workoutExerciseService)
        {
            _operationStackedContext = operationStackedContext;
            _linearProgressionService = linearProgressionService;
            _workoutExerciseService = workoutExerciseService;
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

                foreach (var lpRequest in request.Exercises)
                {
                    // Determine if we need to create a new exercise or use an existing one.
                    Exercise exercise =
                        await _exerciseCreationService.CreateExercise(lpRequest.WorkoutExercise.Exercise);

                    // Create the WorkoutExercise.
                    WorkoutExercise workoutExercise = await _workoutExerciseService.CreateWorkoutExercise(
                        new CreateWorkoutExerciseRequest()
                        {
                            ExerciseId = exercise.Id,
                            Template = lpRequest.WorkoutExercise.Template,
                            LiftDay = lpRequest.WorkoutExercise.LiftDay,
                            LiftOrder = lpRequest.WorkoutExercise.LiftOrder,
                            RestTimer = lpRequest.WorkoutExercise.RestTimer,
                        });


                   var linearProgressionExercise = await _linearProgressionService.CreateLinearProgressionExercise(
                        lpRequest, workoutExercise, request.UserId);

                    workoutExercise.LinearProgressionExercise = linearProgressionExercise;

                    if (linearProgressionExercise != null)
                    {
                        linearProgressionExercises.Add(linearProgressionExercise);
                    }
                }

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
