using OperationStacked.Models;

namespace OperationStacked.Requests;

public class CreateWorkoutExerciseRequest
{
    public int LiftDay { get; set; }
    public int LiftOrder { get; set; }
    public CreateExerciseRequest Exercise { get; set; }
    public Guid ExerciseId { get; set; }
    public ExerciseTemplate Template { get; set; }
    public int RestTimer { get; set; }
    public Guid WorkoutId { get; set; }
    public Guid EquipmentStackId { get; set; }
    public decimal WeightProgression { get; set; }
    public int MinimumReps { get; set; }
    public int MaximumReps { get; set; }
    public int Sets { get; set; }
    public int AttemptsBeforeDeload { get; set; }
}
