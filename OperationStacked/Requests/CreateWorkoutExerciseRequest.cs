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
}
