using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Repositories;
using OperationStacked.Requests;

namespace OperationStacked.Factories
{
    public abstract class ExerciseFactory<T> : IExerciseFactory where T : Exercise, new()
    {
        public ExerciseFactory(IExerciseRepository exerciseRepository)
        {

            _exerciseRepository = exerciseRepository;

        }
        protected T _exercise;
        protected CreateExerciseModel _createExerciseModel;
        protected IExerciseRepository _exerciseRepository;

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
                WorkingWeight = _createExerciseModel.StartingWeight,
                ParentId = _createExerciseModel.ParentId
            };

        }
        public abstract IExercise CreateExercise(CreateExerciseModel createExerciseModel);
        public abstract bool AppliesTo(Type type);

        public abstract Task<(Exercise, ExerciseCompletedStatus)> ProgressExercise(CompleteExerciseRequest request);

    }
}
