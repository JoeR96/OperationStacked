using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;

namespace OperationStacked.Abstractions
{
    public interface IExerciseFactory
    {
        public Exercise CreateExercise(CreateExerciseModel request);

    }
}
