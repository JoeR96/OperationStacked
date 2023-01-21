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
        private readonly IA2SHypertrophyService _a2SHypertrophyService;
         
        public ExerciseCreationService(
            IA2SHypertrophyService a2SHypertrophyService, OperationStackedContext operationStackedContext)
        {
            _a2SHypertrophyService = a2SHypertrophyService;
            _operationStackedContext = operationStackedContext;
        }

        public async Task<Result<WorkoutCreationResult>> CreateWorkout(CreateWorkoutRequest request)
        {
            var strategy = new ExerciseStrategy(new IExerciseFactory[] {
            new A2SHypertrophyFactory(_a2SHypertrophyService),
            new LinearProgressionFactory()
            });

            IEnumerable<IExercise> exercises
                = request.ExerciseDaysAndOrders.Select(exercise => strategy.CreateExercise(ResolveType(exercise.Template),exercise));

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
