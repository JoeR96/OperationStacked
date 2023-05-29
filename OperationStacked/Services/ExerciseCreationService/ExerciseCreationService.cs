using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStacked.Strategy;

namespace OperationStacked.Services.ExerciseCreationService
{
    public class ExerciseCreationService : IExerciseCreationService
    {
        private readonly OperationStackedContext _operationStackedContext;
        private readonly IExerciseStrategyResolver _exerciseStrategyResolver;
         
        public ExerciseCreationService(
            IExerciseStrategyResolver exerciseStrategyResolver, 
            OperationStackedContext operationStackedContext)
        {
            _exerciseStrategyResolver = exerciseStrategyResolver;
            _operationStackedContext = operationStackedContext;
        }

        public async Task<WorkoutCreationResult> CreateWorkout(CreateWorkoutRequest request)
        {
            IEnumerable<IExercise> exercises = await Task.WhenAll(
                request.ExerciseDaysAndOrders.Select(exercise => _exerciseStrategyResolver
                    .CreateStrategy()
                    .CreateExercise(ResolveType(exercise.Template), exercise))
            );



            await _operationStackedContext.AddRangeAsync(exercises);
            await _operationStackedContext.SaveChangesAsync();

            // Cast each item in the exercises collection individually
            var castExercises = exercises
                .Select(exercise => exercise as Exercise)
                .Where(exercise => exercise != null);

            return new WorkoutCreationResult(
                castExercises.Any() ? WorkoutCreatedStatus.Created : WorkoutCreatedStatus.Error,
                castExercises);
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
