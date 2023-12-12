using OperationStacked.Enums;
using OperationStacked.Models;

namespace OperationStacked.DTOs;

public class LinearProgressionExerciseDto
{
    public Guid WorkoutExerciseId { get; set; }
    public Guid Id { get; set; }
    public int CurrentAttempt { get; set; }
    public Guid ParentId { get; set; }
    public int LiftWeek { get; set; }
    public decimal WorkingWeight { get; set; }
    public int WeightIndex { get; set; }
}

public class ExerciseDto
{
    public Guid Id { get; set; }
    public string ExerciseName { get; set; }
    public Category Category { get; set; }
    public EquipmentType EquipmentType { get; set; }
    public Guid UserId { get; set; }
}

public class WorkoutExerciseDto
{
    public Guid Id { get; set; }
    public Guid WorkoutId { get; set; }
    public Guid ExerciseId { get; set; }
    public ExerciseDto Exercise { get; set; }
    public ICollection<LinearProgressionExerciseDto> LinearProgressionExercises { get; set; }
    public ExerciseTemplate Template { get; set; }
    public int LiftDay { get; set; }
    public int LiftOrder { get; set; }
    public bool Completed { get; set; }
    public int RestTimer { get; set; }
    public int MinimumReps { get; set; }
    public int MaximumReps { get; set; }
    public int Sets { get; set; }
    public decimal WeightProgression { get; set; }
    public int AttemptsBeforeDeload { get; set; }
    public Guid EquipmentStackId { get; set; }
}
