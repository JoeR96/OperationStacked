namespace OperationStacked.TestLib.Adapters;

public static class ExerciseAdapter
{
    public static OperationStacked.Entities.Exercise AdaptToEntity(this Exercise original)
    {
        return new OperationStacked.Entities.Exercise
        {
            Id = original.Id,
            ExerciseName = original.ExerciseName,
            Category = (Enums.Category)original.Category,
            EquipmentType = (Enums.EquipmentType)original.EquipmentType,
            UserId = original.UserId
        };
    }

    public static OperationStacked.TestLib.CreateExerciseRequest AdaptToCreateExerciseRequest(this Exercise original)
    {
        return new ()
        {
            ExerciseName = original.ExerciseName,
            Category = (Category)original.Category,
            EquipmentType = (EquipmentType)original.EquipmentType,
            UserId = original.UserId
        };
    }
    public static Requests.CreateExerciseRequest Adapt(CreateExerciseRequest jsonRequest)
    {
        return new OperationStacked.Requests.CreateExerciseRequest
        {
            ExerciseName = jsonRequest.ExerciseName,
            Category = (Enums.Category)jsonRequest.Category,
            EquipmentType = (Enums.EquipmentType)jsonRequest.EquipmentType,
            UserId = jsonRequest.UserId
        };
    }

    public static Requests.CreateExerciseRequest AdaptToConcrete(this CreateExerciseRequest jsonRequest)
    {
        return new OperationStacked.Requests.CreateExerciseRequest
        {
            ExerciseName = jsonRequest.ExerciseName,
            Category = (Enums.Category)jsonRequest.Category,
            EquipmentType = (Enums.EquipmentType)jsonRequest.EquipmentType,
            UserId = jsonRequest.UserId
        };
    }
}
