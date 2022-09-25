using OperationStacked.Entities;
using OperationStacked.Models;

namespace OperationStacked.Response
{
    public sealed record ExerciseCompletionResult(ExerciseCompletedStatus Status,Exercise exercise);
}
