using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OperationStacked.Data;
using OperationStacked.Factories;
using OperationStacked.Options;
using OperationStacked.Repositories;
using OperationStacked.Repositories.WorkoutRepository;
using OperationStacked.Services;
using OperationStacked.Services.A2S;
using OperationStacked.Services.ExerciseCreationService;
using OperationStacked.Services.ExerciseProgressionService;
using OperationStacked.Services.ExerciseRetrievalService;
using OperationStacked.Services.UserAccountsService;
using OperationStacked.Services.WorkoutService;

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
                .AddTransient<IWorkoutExerciseProgressionService, WorkoutExerciseProgressionService>()
                .AddTransient<IExerciseCreationService, ExerciseCreationService>()
                .AddTransient<IExerciseRetrievalService, ExerciseRetrievalService>()
                .AddTransient<IUserAccountService, UserAccountService>()
                .AddTransient<IA2SHypertrophyService, A2SHypertrophyService>()
                .AddTransient<IWorkoutExerciseService, WorkoutExerciseService>()
                .AddTransient<LinearProgressionService>()
                .AddTransient<IExerciseHistoryService, ExerciseHistoryService>()
                .AddTransient<IWorkoutService, WorkoutService>();

        public static IServiceCollection AddRepositories(this IServiceCollection services)
            => services.AddTransient<IExerciseRepository, ExerciseRepository>()
                .AddTransient<IWorkoutRepository, WorkoutRepository>()
                .AddTransient<IEquipmentStackRepository, EquipmentStackRepository>(); // Added line

        public static IServiceCollection AddOperationStackedContext(this IServiceCollection services)
        {
            services.AddDbContext<OperationStackedContext>((serviceProvider, options) =>
            {
                var connectionStringOptions = serviceProvider.GetRequiredService<IOptions<ConnectionStringOptions>>();
                options.UseMySql(connectionStringOptions.Value.ConnectionString, new MySqlServerVersion(new Version(8, 0, 29)));
            }, ServiceLifetime.Transient);

            services.AddDbContextFactory<OperationStackedContext>((serviceProvider, options) =>
            {
                var connectionStringOptions = serviceProvider.GetRequiredService<IOptions<ConnectionStringOptions>>();
                options.UseMySql(connectionStringOptions.Value.ConnectionString, new MySqlServerVersion(new Version(8, 0, 29)));
            }, ServiceLifetime.Transient);

            return services;
        }
    }

}
