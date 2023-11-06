using OperationStacked.Enums;

namespace OperationStacked.Requests;

public class CreateExerciseRequest
{
    public string ExerciseName { get; set; }
    public Category Category { get; set; }
    public EquipmentType EquipmentType { get; set; }
    public Guid UserId { get; set; }
}
