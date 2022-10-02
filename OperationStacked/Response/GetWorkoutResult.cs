using OperationStacked.Entities;

namespace OperationStacked.Response
{
    public sealed record GetWorkoutResult(IEnumerable<Exercise> Exercises);

}
