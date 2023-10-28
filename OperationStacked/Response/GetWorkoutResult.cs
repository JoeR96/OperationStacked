using OperationStacked.Entities;

namespace OperationStacked.Response
{
    public sealed record GetWorkoutResult
    {
        public IEnumerable<WorkoutExercise> Exercises { get; init; }
        public int? TotalCount { get; init; }  // Make TotalCount nullable
        
        public GetWorkoutResult(IEnumerable<WorkoutExercise> exercises, int? totalCount = null)
        {
            Exercises = exercises;
            TotalCount = totalCount;
        }
    }
}