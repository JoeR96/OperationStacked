using OperationStacked.Entities;
using OperationStacked.Enums;

namespace OperationStacked.Requests;

public class CreateLinearProgressionExerciseRequest
{
    public int MinimumReps { get; set; }
    public int MaximumReps { get; set; }
    public int TargetSets { get; set; }
    public int WeightIndex { get; set; }
    public decimal WeightProgression { get; set; }
    public int AttemptsBeforeDeload { get; set; }
    public EquipmentType EquipmentType { get; set; }
    public EquipmentStackKey EquipmentStackKey { get; set; }
    public CreateEquipmentStackRequest EquipmentStack { get; set; }
    public CreateWorkoutExerciseRequest WorkoutExercise { get; set; }
}
