using OperationStacked.Abstractions;
using OperationStacked.Entities;
using OperationStacked.Factories;

namespace OperationStacked.Extensions.FactoryExtensions
{
    public static class ExerciseFactoryExtensions
    {
        public static void AddExerciseFactory(this IServiceCollection services)
        {
            services.AddSingleton<Func<IEnumerable<IExercise>>>
                (x => () => x.GetService<IEnumerable<IExercise>>()!);

        }
    }

}
