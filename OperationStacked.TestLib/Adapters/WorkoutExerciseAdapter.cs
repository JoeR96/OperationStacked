using OperationStacked.Requests;

namespace OperationStacked.TestLib.Adapters;

public static class WorkoutExerciseAdapter
{
    public static OperationStacked.Entities.WorkoutExercise AdaptToEntity(this WorkoutExercise original)
    {
        var newWorkoutExercise = new OperationStacked.Entities.WorkoutExercise
        {
            Id = original.Id,
            WorkoutId = original.WorkoutId,
            ExerciseId = original.ExerciseId,
            Exercise = original.Exercise.AdaptToEntity(),
            LinearProgressionExercise = original.LinearProgressionExercise.AdaptToEntity(), // Use the previously defined AdaptToEntity method for LinearProgressionExercise
            Template = (Models.ExerciseTemplate)original.Template,
            LiftDay = original.LiftDay,
            LiftOrder = original.LiftOrder,
            Completed = original.Completed,
            RestTimer = original.RestTimer
        };

        newWorkoutExercise.LinearProgressionExercise.WorkoutExerciseId = newWorkoutExercise.Id;

        return newWorkoutExercise;
    }

    public static CreateWorkoutExerciseRequest AdaptToCreateRequest(this WorkoutExercise original)
    {
        return new CreateWorkoutExerciseRequest
        {
            LiftDay = original.LiftDay,
            LiftOrder = original.LiftOrder,
            ExerciseId = original.ExerciseId,
            Template = original.Template,
            RestTimer = original.RestTimer,
            Exercise = original.Exercise.AdaptToCreateExerciseRequest() // Assuming there's a method to adapt Exercise to CreateExerciseRequest
        };
    }

    public static OperationStacked.Requests.CreateWorkoutExerciseRequest Adapt(this OperationStacked.TestLib.CreateWorkoutExerciseRequest jsonRequest)
    {
        return new OperationStacked.Requests.CreateWorkoutExerciseRequest
        {
            LiftDay = jsonRequest.LiftDay,
            LiftOrder = jsonRequest.LiftOrder,
            Exercise = jsonRequest.Exercise.AdaptToConcrete(),
            ExerciseId = jsonRequest.ExerciseId,
            Template = (Models.ExerciseTemplate)jsonRequest.Template,
            RestTimer = jsonRequest.RestTimer
        };
    }

}
