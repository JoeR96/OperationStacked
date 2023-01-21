using OperationStacked.Factories;
using OperationStacked.Repositories;
using OperationStacked.Services.A2S;

namespace OperationStacked.Strategy
{
    public class ExerciseStrategyResolver : IExerciseStrategyResolver
    {
        private readonly IA2SHypertrophyService _a2SHypertrophyService;
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseStrategyResolver(IExerciseRepository exerciseRepository, IA2SHypertrophyService a2SHypertrophyService)
        {
            _exerciseRepository = exerciseRepository;
            _a2SHypertrophyService = a2SHypertrophyService;

        }

        public ExerciseStrategy CreateStrategy() => new ExerciseStrategy(new IExerciseFactory[] {
            new A2SHypertrophyFactory(
                _a2SHypertrophyService,
                _exerciseRepository),
            new LinearProgressionFactory(
                _exerciseRepository)
            });

    }
}
