using FluentResult;
using Microsoft.EntityFrameworkCore;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStacked.Strategy;

namespace OperationStacked.Services.ExerciseProgressionService
{
    public class ExerciseProgressionService : IExerciseProgressionService
    {
        private readonly OperationStackedContext _operationStackedContext;
        private readonly IExerciseStrategyResolver _exerciseStrategyResolver;

        public ExerciseProgressionService(
            IExerciseStrategyResolver exerciseStrategyResolver,
            OperationStackedContext operationStackedContext)
        {
            _exerciseStrategyResolver = exerciseStrategyResolver;
            _operationStackedContext = operationStackedContext;
        }

        public async Task<Result<ExerciseCompletionResult>> CompleteExercise(CompleteExerciseRequest request)
        {
            var exercise = GetExercise(request.Id);
            var strategy = _exerciseStrategyResolver.CreateStrategy();
            var (status,nextExercise) = await strategy.ProgressExercise(ResolveType(exercise.Template), request, exercise);
            return Result.Create(new ExerciseCompletionResult(nextExercise, status));

        }

        private Exercise GetExercise(Guid id)
            => _operationStackedContext
                .Exercises
                .Where(x => x.Id == id)
                .FirstOrDefault();

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
