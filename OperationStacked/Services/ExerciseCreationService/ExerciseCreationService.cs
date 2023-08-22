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
        public ExerciseCreationService(
            OperationStackedContext operationStackedContext, LinearProgressionService linearProgressionService)
        {
            _operationStackedContext = operationStackedContext;
            _linearProgressionService = linearProgressionService;
        }

        public async Task<WorkoutCreationResult> CreateWorkout(CreateWorkoutRequest request)
        {
            IEnumerable<Exercise> exercises = await Task.WhenAll(
                request.ExerciseDaysAndOrders.Select(async exercise =>
                {
                    switch (exercise.Template)
                    {
                        case ExerciseTemplate.LinearProgression:
                            return await _linearProgressionService.CreateExercise(exercise,request.userId);
                        case ExerciseTemplate.A2SHypertrophy:
                            // Call the appropriate service method for A2SHypertrophy
                            // For now, return null or a default value until you implement this
                            return null;
                        default:
                            throw new InvalidOperationException($"Unsupported exercise template: {exercise.Template}");
                    }
                })
            );


            await _operationStackedContext.AddRangeAsync(exercises);
            await _operationStackedContext.SaveChangesAsync();

            // Cast each item in the exercises collection individually
            // var castExercises = exercises
            //     .Select(exercise => exercise as Exercise)
            //     .Where(exercise => exercise != null);
            //
            return new WorkoutCreationResult(
                exercises.Any() ? WorkoutCreatedStatus.Created : WorkoutCreatedStatus.Error,
                exercises);
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
