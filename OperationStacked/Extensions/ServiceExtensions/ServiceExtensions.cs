using System.Reflection;
using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Repositories;
using OperationStacked.Services;
using OperationStacked.Services.A2S;
using OperationStacked.Services.A2S.ToDoService;
using OperationStacked.Services.ExerciseCreationService;
using OperationStacked.Services.ExerciseProgressionService;
using OperationStacked.Services.ExerciseRetrievalService;
using OperationStacked.Services.UserAccountsService;
using OperationStacked.Strategy;

namespace OperationStacked.Extensions.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddSingletonsByConvention(this IServiceCollection services, Assembly assembly, Func<Type, bool> interfacePredicate, Func<Type, bool> implementationPredicate)
        {
            var interfaces = assembly.ExportedTypes
                .Where(x => x.IsInterface && interfacePredicate(x))
                .ToList();

            var implementations = assembly.ExportedTypes
                .Where(x => !x.IsInterface && !x.IsAbstract && implementationPredicate(x))
                .ToList();

            foreach (var @interface in interfaces)
            {
                var implementation = implementations.FirstOrDefault(x => @interface.IsAssignableFrom(x));
                if (implementation == null) continue;
                services.AddTransient(@interface, implementation);
            }
            return services;
        }

        public static IServiceCollection AddSingletonsByConvention(this IServiceCollection services, Assembly assembly, Func<Type, bool> predicate)
            => services.AddSingletonsByConvention(assembly, predicate, predicate);

        public static IServiceCollection AddServices(this IServiceCollection services)
            => services
                .AddTransient<IExerciseProgressionService, ExerciseProgressionService>()
                .AddTransient<IExerciseCreationService, ExerciseCreationService>()
                .AddTransient<IExerciseRetrievalService, ExerciseRetrievalService>()
                .AddTransient<IToDoRepository, ToDoRepsitory>()
                .AddTransient<IToDoService, ToDoService>()
                .AddTransient<IUserAccountService, UserAccountService>();

        public static IServiceCollection AddExerciseStrategy(this IServiceCollection services) =>
            services.AddTransient<IExerciseStrategyResolver, ExerciseStrategyResolver>();

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        => services.AddTransient<IExerciseRepository, ExerciseRepository>();
        public static IServiceCollection RegisterA2S(this IServiceCollection services)
        {
            services.AddTransient<IA2SHypertrophyService, A2SHypertrophyService>();

            return services;
        }


    }

}



