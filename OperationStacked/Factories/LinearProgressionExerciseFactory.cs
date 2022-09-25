using OperationStacked.Abstractions;
using OperationStacked.Entities;
using OperationStacked.Models;

namespace OperationStacked.Factories
{
    public class LinearProgressionExerciseFactory : IExerciseFactory
    {
    public Exercise CreateExercise(CreateExerciseModel request)
        {
            Enum.TryParse(request.Template, out ExerciseTemplate template);
            
            return new LinearProgressionExercise
            {
                ExerciseName = request.ExerciseName,
                Username = request.Username,
                Category = request.Category,
                Template = template,
                LiftDay = request.LiftDay,
                LiftOrder = request.LiftOrder,
                MinimumReps = request.MinimumReps,
                MaximumReps = request.MaximumReps,
                TargetSets = request.TargetSets,
                WeightIndex = request.WeightIndex,
                PrimaryExercise = request.PrimaryExercise,
                WorkingWeight = request.WorkingWeight,
                WeightProgression = request.WeightProgression,
                AttemptsBeforeDeload = request.AttemptsBeforeDeload
            };
        }
        
    }
}
