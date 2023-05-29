using OperationStacked.Abstractions;
using OperationStacked.Entities;
using OperationStacked.Models;

namespace OperationStacked.Response
{
    public sealed record WorkoutCreationResult(WorkoutCreatedStatus Status, IEnumerable<Exercise> Exercises);

}
