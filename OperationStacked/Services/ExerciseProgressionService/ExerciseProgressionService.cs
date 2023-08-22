using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Factories;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Services.ExerciseProgressionService;

public class ExerciseProgressionService : IExerciseProgressionService
{
    private readonly OperationStackedContext _operationStackedContext;
    private readonly LinearProgressionService _linearProgressionService;
    public ExerciseProgressionService(
        OperationStackedContext operationStackedContext, LinearProgressionService linearProgressionService)
    {
        _operationStackedContext = operationStackedContext;
        _linearProgressionService = linearProgressionService;
    }

    public async Task<ExerciseCompletionResult> CompleteExercise(CompleteExerciseRequest request)
    {
        var exercise = GetExercise(request.Id);
    
        switch (exercise.Template)
        {
            case ExerciseTemplate.LinearProgression:
                return await HandleLinearProgression(request);

            case ExerciseTemplate.A2SHypertrophy:
                return await HandleA2SHypertrophy(request);

            default:
                throw new InvalidOperationException($"Unsupported exercise template: {exercise.Template}");
        }
    }

    private async Task<ExerciseCompletionResult> HandleLinearProgression(CompleteExerciseRequest request)
    {
        var (nextExercise, status) = await _linearProgressionService.ProgressExercise(request);
        return new ExerciseCompletionResult(status, nextExercise);
    }

    private async Task<ExerciseCompletionResult> HandleA2SHypertrophy(CompleteExerciseRequest request)
    {
        // Implement this when ready
        // For now:
        return new ExerciseCompletionResult(default, default);
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