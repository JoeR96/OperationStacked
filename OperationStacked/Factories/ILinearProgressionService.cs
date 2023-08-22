using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;

namespace OperationStacked.Factories;

public interface ILinearProgressionService
{
    Task<LinearProgressionExercise> CreateExercise(CreateExerciseModel createExerciseModel);
    Task<(LinearProgressionExercise, ExerciseCompletedStatus)> ProgressExercise(CompleteExerciseRequest request);
}