using FluentResult;
using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Factories;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStacked.Services.A2S;
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

        public async Task<Result<WorkoutCreationResult>> CreateWorkout(CreateWorkoutRequest request)
        {
            
            IEnumerable<IExercise> exercises
                = request.ExerciseDaysAndOrders.Select(exercise => _exerciseStrategyResolver
                .CreateStrategy()
                .CreateExercise(ResolveType(exercise.Template),exercise));

            //move this to repository
            await _operationStackedContext.AddRangeAsync(exercises);
            await _operationStackedContext.SaveChangesAsync();

            return Result.Create(new WorkoutCreationResult(
                exercises.Any() ? WorkoutCreatedStatus.Created : WorkoutCreatedStatus.Error,
                exercises));

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
