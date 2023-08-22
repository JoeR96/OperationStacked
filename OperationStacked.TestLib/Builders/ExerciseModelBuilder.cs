using System;
using OperationStacked.Entities;
using OperationStacked.Enums;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.TestLib.Builders;

public class ExerciseModelBuilder
{
    private CreateExerciseModel _model = new();

    // Default values initializer
    public ExerciseModelBuilder WithDefaultValues()
    {
        _model = new CreateExerciseModel()
        {
            Template = ExerciseTemplate.LinearProgression,
            LiftDay = 1,
            LiftOrder = 1,
            UserId = Guid.NewGuid(),
            EquipmentType = EquipmentType.Barbell,
            ExerciseName = "Default Exercise",
            Category = "Default Category",
            MinimumReps = 8,
            MaximumReps = 12,
            TargetSets = 3,
            PrimaryExercise = true,
            StartingWeight = 60m,
            WeightProgression = 2.50m,
            AttemptsBeforeDeload = 2,
            Week = 1
        };
        return this;
    }

    // Properties
    public ExerciseModelBuilder WithEquipmentType(EquipmentType equipmentType)
    {
        _model.EquipmentType = equipmentType;

        // if (equipmentType is EquipmentType.Cable || equipmentType is EquipmentType.Machine)
        // {
        //     _model.EquipmentStack = new EquipmentStackBuilder().WithDefaultValues().Adapt();
        // }
        
        return this;
    }

    public ExerciseModelBuilder WithTemplate(ExerciseTemplate exerciseTemplate)
    {
        _model.Template = exerciseTemplate;
        return this;
    }

    public ExerciseModelBuilder WithWeightProgression(decimal weightProgression)
    {
        _model.WeightProgression = weightProgression;
        return this;
    }

    public ExerciseModelBuilder WithReps(int minReps, int maxReps)
    {
        _model.MinimumReps = minReps;
        _model.MaximumReps = maxReps;
        return this;
    }

    // Additional builder methods can be added here based on your requirements

    // Build the model
    public CreateExerciseModel Build() => _model;
}