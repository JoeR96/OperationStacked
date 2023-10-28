using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Factories;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Services.ExerciseCreationService
{
    public class ExerciseCreationService : IExerciseCreationService
    {
        private readonly OperationStackedContext _operationStackedContext;
        private readonly LinearProgressionService _linearProgressionService;
        private readonly IWorkoutExerciseService _workoutExerciseService;
        public ExerciseCreationService(
            OperationStackedContext operationStackedContext, LinearProgressionService linearProgressionService)
        {
            _operationStackedContext = operationStackedContext;
            _linearProgressionService = linearProgressionService;
        }

        public async Task<WorkoutCreationResult> CreateWorkout(CreateWorkoutRequest request)
        {
            var workoutExercises =await Task.WhenAll(
                request.ExerciseDaysAndOrders.Select(async exercise => await  _workoutExerciseService.CreateWorkoutExercise(exercise)));


            //exercises are already created so we create the first week for each template
            var linearProgressionExercises = await Task.WhenAll(
                workoutExercises.Select(async workoutExercise =>
                {
                    var exerciseModel = request.ExerciseDaysAndOrders.First(e => e.ExerciseId == workoutExercise.ExerciseId);

                    switch (exerciseModel.Template)
                    {
                        case ExerciseTemplate.LinearProgression:
                            var linearProgressionExercise = await _linearProgressionService.CreateLinearProgressionExercise(exerciseModel, request.userId);

                            // Link WorkoutExercise to LinearProgressionExercise
                            linearProgressionExercise.WorkoutExerciseId = workoutExercise.ExerciseId; // or perhaps workoutExercise.Id depending on your structure
                            workoutExercise.LinearProgressionExercise = linearProgressionExercise;

                            return linearProgressionExercise;

                        case ExerciseTemplate.A2SHypertrophy:
                            return null;

                        default:
                            throw new InvalidOperationException($"Unsupported exercise template: {exerciseModel.Template}");
                    }
                })
            );


            await _operationStackedContext.SaveChangesAsync();

            // Cast each item in the exercises collection individually
            // var castExercises = exercises
            //     .Select(exercise => exercise as Exercise)
            //     .Where(exercise => exercise != null);
            //
            return new WorkoutCreationResult(
                workoutExercises.Any() ? WorkoutCreatedStatus.Created : WorkoutCreatedStatus.Error,
                (linearProgressionExercises));
        }


        private Type ResolveType(ExerciseTemplate template)
        {
            switch (template)
            {
                case ExerciseTemplate.A2SHypertrophy:
                    return typeof(A2SHypertrophyExercise);
                case ExerciseTemplate.LinearProgression:
                    return typeof(LinearProgressionExercise);
                default:
                    throw new ArgumentException($"Type of {template} not registered");
            }
        }
    }


}
