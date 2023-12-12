using OperationStacked.Enums;

namespace OperationStacked.DTOs;

public class ExerciseHistoryDTO
{
    public Guid Id { get; set; }
    public DateTime CompletedDate { get; set; }
    public int CompletedSets { get; set; }
    public string CompletedReps { get; set; }
    public Guid ExerciseId { get; set; }
    public ExerciseDTO Exercise { get; set; }
    public Guid? TemplateExerciseId { get; set; }
    public decimal WorkingWeight { get; set; }
}

public class ExerciseDTO
{
    public Guid Id { get; set; }
    public string ExerciseName { get; set; }
    public Category Category { get; set; }
    public EquipmentType EquipmentType { get; set; }
}
