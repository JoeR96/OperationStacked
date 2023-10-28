using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Enums;
using OperationStacked.Extensions.TemplateExtensions;
using OperationStacked.Factories;
using OperationStacked.Models;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Services.ExerciseProgressionService;

public class ExerciseProgressionService : IExerciseProgressionService
{
    private readonly OperationStackedContext _operationStackedContext;
    private readonly LinearProgressionService _linearProgressionService;
    private readonly IExerciseRepository _exerciseRepository;
    public ExerciseProgressionService(
        OperationStackedContext operationStackedContext, 
        LinearProgressionService linearProgressionService, 
        IExerciseRepository exerciseRepository)
    {
        _operationStackedContext = operationStackedContext;
        _linearProgressionService = linearProgressionService;
        _exerciseRepository = exerciseRepository;
    }

    public async Task<ExerciseCompletionResult> CompleteExercise(CompleteExerciseRequest request)
    {
        var exercise = GetWorkoutExercise(request.ExerciseId);


        if (request.Template != null)
        {
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

        return new ExerciseCompletionResult(ExerciseCompletedStatus.Progressed, null);
    }

    public async Task<LinearProgressionExercise> UpdateWorkingWeight(UpdateExerciseRequest request)
    {
        var exercise = await _exerciseRepository.GetLinearProgressionExerciseById(request.Id);
        var success = false;
        
        if (exercise.WorkoutExercise.Exercise.EquipmentType is EquipmentType.Barbell or EquipmentType.SmithMachine or EquipmentType.Dumbbell or EquipmentType.SmithMachine)
        {
            exercise.WorkingWeight = request.WorkingWeight;
        }

        if (exercise.WorkoutExercise.Exercise.EquipmentType is EquipmentType.Cable or EquipmentType.Machine)
        {
            var stackId = exercise.EquipmentStackId;
            var equipmentStack = await _exerciseRepository.GetEquipmentStack(stackId);
            var generatedStack = equipmentStack.GenerateStack();


            var newIndex = FindClosestIndex(generatedStack, request.WorkingWeight);
            var newWeight = (decimal)generatedStack[newIndex];
            exercise.WorkingWeight = newWeight;
            exercise.WeightIndex = newIndex;
            await _exerciseRepository.UpdateExerciseById(request, newIndex);
        }
        exercise.MaximumReps = request.MaximumReps;
        exercise.MinimumReps = request.MinimumReps;
        exercise.Sets = request.Sets;
        await _exerciseRepository.UpdateExerciseById(request);
        return exercise;
        
    }
    private static int FindClosestIndex(decimal?[] arr, decimal target) 
    {
        int closestIndex = 0;
        decimal closestDiff = Math.Abs((decimal)(arr[0] - target));

        for (int i = 1; i < arr.Length; i++)
        {
            decimal currentDiff = Math.Abs((decimal)(arr[i] - target));
            if (currentDiff < closestDiff)
            {
                closestDiff = currentDiff;
                closestIndex = i;
            }
        }

        return closestIndex;
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

    private WorkoutExercise GetWorkoutExercise(Guid id)
        => _operationStackedContext
            .WorkoutExercises
            .Where(x => x.Exercise.Id == id)
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
