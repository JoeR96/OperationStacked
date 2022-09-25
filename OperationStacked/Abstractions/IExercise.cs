using OperationStacked.Models;

namespace OperationStacked.Abstractions
{
    public interface IExercise
    {
        ExerciseTemplate Template { get; }
    }
}
