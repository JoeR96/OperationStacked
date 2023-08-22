using System;
using OperationStacked.Entities;
using OperationStacked.Enums;
using OperationStacked.Models;

public class ExerciseBuilder<T, TB> where T : Exercise, new() where TB : ExerciseBuilder<T, TB>, new()
{
    protected T _exercise = new();
    protected virtual TB Self() => (TB)this;

    public virtual TB WithDefaultValues()
    {
        _exercise = new T()
        {
            ExerciseName = "Default Exercise",
            Category = "Default Category",
            EquipmentType = EquipmentType.Barbell,
            Template = ExerciseTemplate.LinearProgression,
            LiftDay = 1,
            LiftOrder = 1,
            LiftWeek = 1,
            WorkingWeight = 60.00M,
            Completed = false,
            RestTimer = 0
        };
        return Self();
    }

    public virtual TB WithEquipmentType(EquipmentType equipmentType)
    {
        _exercise.EquipmentType = equipmentType;
        return Self();
    }

    public  virtual TB WithTemplate(ExerciseTemplate exerciseTemplate)
    {
        _exercise.Template = exerciseTemplate;
        return Self();
    }

    public virtual TB WithWorkingWeight(decimal workingWeight)
    {
        _exercise.WorkingWeight = workingWeight;
        return Self();
    }

    public T Build() => _exercise;
}