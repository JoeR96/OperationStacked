using OperationStacked.TestLib;
using Category = OperationStacked.Enums.Category;
using EquipmentType = OperationStacked.Enums.EquipmentType;
using Exercise = OperationStacked.Entities.Exercise;

public class ExerciseBuilder
{
    protected Exercise _exercise = new Exercise();

    public ExerciseBuilder WithDefaultValues()
    {
        _exercise.ExerciseName = "Default Exercise";
        _exercise.Category = Category.Shoulders;
        _exercise.EquipmentType = EquipmentType.Barbell;
        _exercise.UserId = Guid.Empty; // or some default user id
        return this;
    }

    public ExerciseBuilder WithExerciseName(string name)
    {
        _exercise.ExerciseName = name;
        return this;
    }

    public ExerciseBuilder WithCategory(Category category)
    {
        _exercise.Category = category;
        return this;
    }

    public ExerciseBuilder WithEquipmentType(EquipmentType equipmentType)
    {
        _exercise.EquipmentType = equipmentType;
        return this;
    }

    public ExerciseBuilder WithUserId(Guid userId)
    {
        _exercise.UserId = userId;
        return this;
    }

    public Exercise Build() => _exercise;

    public CreateExerciseRequest BuildCreateExerciseRequest()
    {
        return new CreateExerciseRequest
        {
            ExerciseName = _exercise.ExerciseName,
            Category = (OperationStacked.TestLib.Category)_exercise.Category,
            EquipmentType = (OperationStacked.TestLib.EquipmentType)_exercise.EquipmentType,
            UserId = _exercise.UserId
        };
    }
}
