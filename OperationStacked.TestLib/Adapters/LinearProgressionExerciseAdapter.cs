using OperationStacked.Requests;
using OperationStacked.TestLib.Builders;

namespace OperationStacked.TestLib.Adapters;

public static  class LinearProgressionExerciseAdapter
{
    public static OperationStacked.Entities.LinearProgressionExercise AdaptToEntity(this LinearProgressionExercise original)
    {
        return new OperationStacked.Entities.LinearProgressionExercise
        {
            WorkoutExerciseId = original.WorkoutExerciseId,
            WorkoutExercise = original.WorkoutExercise.AdaptToEntity(),
            Id = original.Id,
            MinimumReps = original.MinimumReps,
            MaximumReps = original.MaximumReps,
            Sets = original.Sets,
            WeightProgression = original.WeightProgression,
            AttemptsBeforeDeload = original.AttemptsBeforeDeload,
            CurrentAttempt = original.CurrentAttempt,
            ParentId = original.ParentId,
            LiftWeek = original.LiftWeek,
            WorkingWeight = original.WorkingWeight,
            WeightIndex = original.WeightIndex,
            EquipmentStackId = original.EquipmentStackId
        };
    }

    public static OperationStacked.Requests.CreateLinearProgressionExerciseRequest AdaptToCreateRequest(this CreateLinearProgressionExerciseRequest original)
    {
        return new  OperationStacked.Requests.CreateLinearProgressionExerciseRequest
        {
            MinimumReps = original.MinimumReps,
            MaximumReps = original.MaximumReps,
            TargetSets = original.TargetSets,
            WeightIndex = original.WeightIndex,
            WeightProgression = original.WeightProgression,
            AttemptsBeforeDeload = original.AttemptsBeforeDeload,
            EquipmentType = (Enums.EquipmentType)original.WorkoutExercise.Exercise.EquipmentType, // You need to define how to get this value
            EquipmentStack =  new EquipmentStackBuilder().WithDefaultValues().BuildCreateRequest().Adapt(),
            WorkoutExercise = original.WorkoutExercise.Adapt()
        };
    }
}
