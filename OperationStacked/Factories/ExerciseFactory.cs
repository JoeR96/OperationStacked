using OperationStacked.Abstractions;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Repositories;
using OperationStacked.Requests;

namespace OperationStacked.Factories
{
    public static class ExerciseExtensions
    {
        public static void PopulateBaseValues(this Exercise e, CreateExerciseModel _createExerciseModel, Guid userId)
        {

            e.ExerciseName = _createExerciseModel.ExerciseName;
            e.Category = _createExerciseModel.Category;
            e.Template = _createExerciseModel.Template;
            e.LiftDay = _createExerciseModel.LiftDay;
            e.LiftOrder = _createExerciseModel.LiftOrder;
            e.UserId = userId;
            e.EquipmentType = _createExerciseModel.EquipmentType;
            e.WorkingWeight = _createExerciseModel.StartingWeight;
            e.RestTimer = _createExerciseModel.RestTimer;
            e.ParentId = _createExerciseModel.ParentId;
        }

    }
}
