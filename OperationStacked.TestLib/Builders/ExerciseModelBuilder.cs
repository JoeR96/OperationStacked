﻿using System;
using OperationStacked.Entities;
using OperationStacked.Enums;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.TestLib.Builders;
using A2SBlocks = OperationStacked.TestLib.A2SBlocks;
using EquipmentStackKey = OperationStacked.TestLib.EquipmentStackKey;

public class ExerciseModelBuilder
{
    private CreateExerciseModel _model = new();

    public ExerciseModelBuilder()
    {
        WithDefaultValues();
    }
    // Default values initializer
    public ExerciseModelBuilder WithDefaultValues()
    {
        _model = new CreateExerciseModel()
        {
            Template = ExerciseTemplate.LinearProgression,
            LiftDay = 1,
            LiftOrder = 1,
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
    public ExerciseModelBuilder WithLiftOrder(int order)
    {
        _model.LiftOrder = order;
        return this;
    }
    public ExerciseModelBuilder WithLiftDay(int day)
    {
        _model.LiftDay = day;
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

    public OperationStacked.TestLib.CreateExerciseModel Adapt()
    {
        try
        {
            return new OperationStacked.TestLib.CreateExerciseModel
            {
                ExerciseName = _model.ExerciseName ?? string.Empty,
                Category = _model.Category ?? string.Empty,
                Template = (OperationStacked.TestLib.ExerciseTemplate)_model.Template,
                EquipmentType = (OperationStacked.TestLib.EquipmentType)_model.EquipmentType,
                LiftDay = _model.LiftDay,
                LiftOrder = _model.LiftOrder,
                MinimumReps = _model.MinimumReps,
                MaximumReps = _model.MaximumReps,
                TargetSets = _model.TargetSets,
                WeightIndex = _model.WeightIndex,
                PrimaryExercise = _model.PrimaryExercise,
                StartingWeight = (double)_model.StartingWeight,
                WeightProgression = (double)_model.WeightProgression,
                AttemptsBeforeDeload = _model.AttemptsBeforeDeload,
                UserId = _model.UserId,
                PrimaryLift = _model.PrimaryLift,
                Block = (A2SBlocks)_model.Block,
                TrainingMax = (double)_model.TrainingMax,
                Intensity = (double)_model.Intensity,
                Sets = _model.Sets,
                RepsPerSet = _model.RepsPerSet,
                Week = _model.Week,
                ParentId = _model.ParentId,
                EquipmentStack = new EquipmentStackBuilder().WithDefaultValues().Adapt(),
                RestTimer = _model.RestTimer
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

 
}