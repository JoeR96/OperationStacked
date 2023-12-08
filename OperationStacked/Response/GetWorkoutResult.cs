using OperationStacked.DTOs;
using OperationStacked.Entities;

namespace OperationStacked.Response
{
    public sealed record GetWorkoutResult
    {
        public IEnumerable<WorkoutExerciseDto> Exercises { get; init; }
        public int? TotalCount { get; init; }  // Make TotalCount nullable
        
        public GetWorkoutResult(IEnumerable<WorkoutExerciseDto> exercises, int? totalCount = null)
        {
            Exercises = exercises;
            TotalCount = totalCount;
        }
    }
}
