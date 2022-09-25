using OperationStacked.Abstractions;
using OperationStacked.Entities;
using OperationStacked.Factories;

namespace OperationStacked.Extensions
{
    public static class ExerciseFactoryExtensions
    {
        public static void AddExerciseFactory(this IServiceCollection services)
        {
            services.AddTransient<Exercise, LinearProgressionExercise>();
            services.AddSingleton<Func<IEnumerable<IExercise>>>
                (x => () => x.GetService<IEnumerable<IExercise>>()!);
            services.AddSingleton<IExerciseFactory, LinearProgressionExerciseFactory>();
        }
    }

}
