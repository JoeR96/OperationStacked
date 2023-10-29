namespace OperationStacked.TestLib.Adapters;

public static class LinearProgressionExerciseMapper
{
    public static Requests.CreateLinearProgressionExerciseRequest Adapt(OperationStacked.TestLib.CreateLinearProgressionExerciseRequest jsonRequest)
    {
        return new OperationStacked.Requests.CreateLinearProgressionExerciseRequest
        {
            MinimumReps = jsonRequest.MinimumReps,
            MaximumReps = jsonRequest.MaximumReps,
            TargetSets = jsonRequest.TargetSets,
            WeightIndex = jsonRequest.WeightIndex,
            WeightProgression = jsonRequest.WeightProgression,
            AttemptsBeforeDeload = jsonRequest.AttemptsBeforeDeload,
            EquipmentType = (Enums.EquipmentType)jsonRequest.EquipmentType,
            EquipmentStackKey = (Enums.EquipmentStackKey)jsonRequest.EquipmentStackKey,
            EquipmentStack = jsonRequest.EquipmentStack.Adapt(),
            WorkoutExercise = jsonRequest.WorkoutExercise.Adapt()
        };
    }
}
