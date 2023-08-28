﻿using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;

namespace OperationStacked.Factories;

public interface ILinearProgressionService
{
    Task<LinearProgressionExercise> CreateExercise(CreateExerciseModel createExerciseModel,
        Guid requestUserId);
    Task<(LinearProgressionExercise, ExerciseCompletedStatus)> ProgressExercise(CompleteExerciseRequest request);

    decimal CreateStack(Guid exerciseParentId, decimal workingWeight, int startIndex,
        EquipmentStack stack);
}