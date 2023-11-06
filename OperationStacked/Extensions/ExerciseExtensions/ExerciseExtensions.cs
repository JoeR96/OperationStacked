using OperationStacked.Abstractions;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using CreateExerciseRequest = OperationStacked.Models.CreateExerciseRequest;

namespace OperationStacked.Factories
{
    public static class ExerciseExtensions
    {
        public static void PopulateBaseValues(this Exercise e, CreateExerciseRequest createExerciseRequest, Guid userId)
        {

            e.ExerciseName = createExerciseRequest.ExerciseName;
            e.Category = createExerciseRequest.Category;
            e.UserId = userId;
            e.EquipmentType = createExerciseRequest.EquipmentType;
        }

    }
}
