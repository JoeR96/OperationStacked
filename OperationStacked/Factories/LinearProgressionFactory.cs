using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Models;

namespace OperationStacked.Factories
{
    public class LinearProgressionFactory : ExerciseFactory<LinearProgressionExercise>
    {
        public override bool AppliesTo(Type type) => typeof(LinearProgressionExercise).Equals(type);

        public override LinearProgressionExercise CreateExercise(CreateExerciseModel createExerciseModel)
        {
            CreateBaseExercise(createExerciseModel);
            _exercise.MinimumReps = _createExerciseModel.MinimumReps;
            _exercise.MaximumReps = _createExerciseModel.MaximumReps;
            _exercise.TargetSets = _createExerciseModel.TargetSets;
            _exercise.WeightIndex = _createExerciseModel.WeightIndex;
            _exercise.PrimaryExercise = _createExerciseModel.PrimaryExercise;
            _exercise.StartingWeight = _createExerciseModel.StartingWeight;
            _exercise.WeightProgression = _createExerciseModel.WeightProgression;
            _exercise.AttemptsBeforeDeload = _createExerciseModel.AttemptsBeforeDeload;


            return _exercise;
        }


    }
}
