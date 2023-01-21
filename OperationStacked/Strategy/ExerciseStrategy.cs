using OperationStacked.Abstractions;
using OperationStacked.Entities;
using OperationStacked.Factories;
using OperationStacked.Models;
using OperationStacked.Requests;
using Org.BouncyCastle.Crypto;

namespace OperationStacked.Strategy
{
    public class ExerciseStrategy
    {
        private readonly IExerciseFactory[] _exerciseFactories;

        public ExerciseStrategy(IExerciseFactory[] exerciseFactories)
        {
            this._exerciseFactories = exerciseFactories ?? throw new ArgumentNullException(nameof(exerciseFactories));
        }

        public IExercise CreateExercise(Type type, Models.CreateExerciseModel exercise)
        {
            var exerciseFactory = _exerciseFactories
                .FirstOrDefault(factory => factory.AppliesTo(type));

            if (exerciseFactory == null)
            {
                throw new InvalidOperationException($"{type} not registered");
            }

            return exerciseFactory.CreateExercise(exercise);
        }
        
        public async Task<(Exercise, ExerciseCompletedStatus)> ProgressExercise(Type type, CompleteExerciseRequest request, Exercise exercise)
        {
            var exerciseFactory = _exerciseFactories
                .FirstOrDefault(factory => factory.AppliesTo(type));

            if (exerciseFactory == null)
            {
                throw new InvalidOperationException($"{type} not registered");
            }

            return await exerciseFactory.ProgressExercise(request);

            

        }
    }
}
