﻿using OperationStacked.Entities;

namespace OperationStacked.Repositories.WorkoutRepository;

public interface IWorkoutRepository
{
      Task<Workout> CreateWorkout(Workout workout);
}
