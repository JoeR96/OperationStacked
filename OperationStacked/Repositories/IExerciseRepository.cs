﻿using OperationStacked.Entities;

namespace OperationStacked.Repositories
{
    public interface IExerciseRepository
    {
        Task<Exercise> GetExerciseById(Guid id);
        Task<List<Exercise>> GetExercises(int userId, int week, int day);
        Task InsertExercise(Exercise nextExercise);
    }
}