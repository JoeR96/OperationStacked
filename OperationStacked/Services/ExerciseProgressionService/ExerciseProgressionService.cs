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

    public async Task<bool> UpdateWorkingWeight(Guid exerciseId, decimal newWorkingWeight)
    {
        var exercise = await _exerciseRepository.GetExerciseById(exerciseId);
        bool success = false;
        if (exercise.EquipmentType is EquipmentType.Barbell or EquipmentType.SmithMachine or EquipmentType.Dumbbell or EquipmentType.SmithMachine)
        {
            exercise.WorkingWeight = newWorkingWeight;
            success = await _exerciseRepository.UpdateExerciseById(exercise.Id, newWorkingWeight);

        }

        if (exercise.EquipmentType is EquipmentType.Cable or EquipmentType.Machine)
        {
            var stackId = exercise.EquipmentStackId;
            var equipmentStack = await _exerciseRepository.GetEquipmentStack(stackId);
            var generatedStack = equipmentStack.GenerateStack();


            var newIndex = FindClosestIndex(generatedStack, newWorkingWeight);
            var newWeight = (decimal)generatedStack[newIndex];
            exercise.WorkingWeight = newWorkingWeight;

            await _exerciseRepository.UpdateExerciseById(exercise.Id, newWeight, newIndex);
            

        }

        success = await _exerciseRepository.UpdateExerciseById(exercise.Id, newWorkingWeight);
        return success;    }
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