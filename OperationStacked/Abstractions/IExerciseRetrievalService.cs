﻿using FluentResult;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Abstractions
{
    public interface IExerciseRetrievalService
    {
        public Task<Result<GetWorkoutResult>> GetWorkout(int userid, int week, int day);
    }
}
