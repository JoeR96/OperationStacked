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
            e.UserId = userId;
            e.EquipmentType = _createExerciseModel.EquipmentType;
        }

    }
}
