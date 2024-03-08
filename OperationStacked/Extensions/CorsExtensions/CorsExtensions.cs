using Microsoft.Extensions.DependencyInjection;

namespace OperationStacked.Extensions.CorsExtensions
{
    public static class CorsExtensions
    {
        public static void AddCustomCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", builder =>
                {
                    builder.WithOrigins(
                            "https://operationstacked.com",
                            "http://localhost:5173",
                            "https://localhost:5173",
                            "http://localhost:4200",
                            "https://your-domain-prefix.auth.eu-west-2.amazoncognito.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
    }
}
