using OperationStacked.Entities;

namespace OperationStacked.Response
{
    public sealed record GetWorkoutResult
    {
        public IEnumerable<Exercise> Exercises { get; init; }
        public int? TotalCount { get; init; }  // Make TotalCount nullable
        
        public GetWorkoutResult(IEnumerable<Exercise> exercises, int? totalCount = null)
        {
            Exercises = exercises;
            TotalCount = totalCount;
        }
    }
}