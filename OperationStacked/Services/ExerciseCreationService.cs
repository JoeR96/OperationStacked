using FluentResult;
using FluentValidation;
using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Models;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStacked.Validators;

namespace OperationStacked.Services
{
    public class ExerciseCreationService : IExerciseCreationService
    {
        private readonly IExerciseFactory _factory;
        private readonly OperationStackedContext _operationStackedContext;

        public ExerciseCreationService(IExerciseFactory factory, OperationStackedContext operationStackedContext)
        {
            _factory = factory;
            _operationStackedContext = operationStackedContext;
        }

        public async Task<Result<WorkoutCreationResult>> CreateWorkout(CreateWorkoutRequest request)
        {
            
            IEnumerable<Exercise> exercises
                = request.ExerciseDaysAndOrders.Select(exercise => _factory.CreateExercise(exercise));

            await _operationStackedContext.AddRangeAsync(exercises);
            await _operationStackedContext.SaveChangesAsync();

            return Result.Create (new WorkoutCreationResult(
                exercises.Any() ? WorkoutCreatedStatus.Created : WorkoutCreatedStatus.Error,
                exercises));
        
        }

 
    }


} 
