using OperationStacked.Abstractions;
using OperationStacked.Factories;
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
    }
}
