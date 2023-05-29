using OperationStacked.Abstractions;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;

namespace OperationStacked.Factories
{
    public interface IExerciseFactory
    {
        bool AppliesTo(Type type);
        Task<IExercise> CreateExercise(CreateExerciseModel exercise);
        abstract Task<(Exercise, ExerciseCompletedStatus)> ProgressExercise(CompleteExerciseRequest request);

    }
}
