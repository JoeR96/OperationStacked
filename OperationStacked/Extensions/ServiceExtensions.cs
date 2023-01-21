using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Services;
using System.Reflection;
using System.Runtime.CompilerServices;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Internal;
using OperationStacked.Utilities;
using ServiceStack.Aws.DynamoDb;
using OperationStacked.Services.ExerciseProgressionService;
using OperationStacked.Repositories;
using OperationStacked.Services.UserAccountsService;
using OperationStacked.Services.JwtService;
using OperationStacked.Services.LoginService;
using OperationStacked.Services.ExerciseCreationService;
using OperationStacked.Services.A2S;

namespace OperationStacked.Extensions
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
        {
            services.AddTransient<IExerciseProgressionService, ExerciseProgressionService>();
            services.AddTransient<IExerciseCreationService, ExerciseCreationService>();
            services.AddTransient<IExerciseRetrievalService, ExerciseRetrievalService>();
            services.AddTransient<IUserAccountService, UserAccountService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IPasswordHasherService, PasswordHasherService>();
            services.AddTransient<ITokenHandlerService, TokenHandlerService>();
            services.AddDbContext<OperationStackedContext>();

            return services;
        }

        public static IServiceCollection RegisterA2S(this IServiceCollection services )
        {
            services.AddTransient<IA2SHypertrophyService, A2SHypertrophyService>();

            return services;
        }
        
        
    }
    
}



