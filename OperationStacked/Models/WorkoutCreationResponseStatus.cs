namespace OperationStacked.Models
{
    public enum WorkoutCreatedStatus
    {
        Created = 1,
        Error = 2
    }

    public enum ExerciseCompletedStatus
    {
        Progressed = 1,
        StayedTheSame = 2,
        Failed = 3,
        Deload = 4,
        Active = 5
    }
}
