using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Models;

namespace OperationStacked.Factories
{
    public abstract class ExerciseFactory<T> : IExerciseFactory where T : Exercise, new()
    {
        protected T _exercise;
        protected CreateExerciseModel _createExerciseModel;

        protected void CreateBaseExercise(CreateExerciseModel createExerciseModel)
        {
            _createExerciseModel = createExerciseModel;
            _exercise = new T()
            {
                ExerciseName = _createExerciseModel.ExerciseName,
                Username = _createExerciseModel.Username,
                Category = _createExerciseModel.Category,
                Template = _createExerciseModel.Template,
                LiftDay = _createExerciseModel.LiftDay,
                LiftOrder = _createExerciseModel.LiftOrder,
                UserId = _createExerciseModel.UserId,
                EquipmentType = _createExerciseModel.EquipmentType,
                WorkingWeight = _createExerciseModel.StartingWeight
            };

        }
        public abstract IExercise CreateExercise(CreateExerciseModel createExerciseModel);

        public abstract bool AppliesTo(Type type);


    }
}
