using OperationStacked.Abstractions;
using OperationStacked.Entities;
using OperationStacked.Models;

namespace OperationStacked.Factories
{
    public class LinearProgressionExerciseFactory : IExerciseFactory
    {
    public LinearProgressionExercise CreateExercise(CreateExerciseModel request)
        {

            return new LinearProgressionExercise
            {
                ExerciseName = request.ExerciseName,
                Username = request.Username,
                Category = request.Category,
                Template = request.Template,
                LiftDay = request.LiftDay,
                LiftOrder = request.LiftOrder,
                MinimumReps = request.MinimumReps,
                MaximumReps = request.MaximumReps,
                TargetSets = request.TargetSets,
                WeightIndex = request.WeightIndex,
                PrimaryExercise = request.PrimaryExercise,
                StartingWeight = request.StartingWeight,
                WeightProgression = request.WeightProgression,
                AttemptsBeforeDeload = request.AttemptsBeforeDeload,
                UserId = request.UserId,
                EquipmentType = request.EquipmentType,
                WorkingWeight = request.StartingWeight
            };
        }
        
    }
}
