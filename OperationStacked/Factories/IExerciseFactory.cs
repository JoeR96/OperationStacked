using OperationStacked.Abstractions;
using OperationStacked.Models;

namespace OperationStacked.Factories
{
    public interface IExerciseFactory
    {
        bool AppliesTo(Type type);
        IExercise CreateExercise(CreateExerciseModel exercise);
    }
}
