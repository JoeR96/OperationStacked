using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OperationStacked.Options;

namespace OperationStacked.Extensions.AuthenticationExtensions;

public static class AuthenticationExtensions
{
    public static AuthenticationBuilder AddCognitoAuthentication(this IServiceCollection services
       )
    {
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var awsOptions = serviceProvider.GetRequiredService<IOptions<AWSOptions>>().Value;
            
            return services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var region = awsOptions.Region;
                    var userPoolId = awsOptions.UserPoolId;
                    options.Authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };

                    // Handle the token received event
                    options.Events = new JwtBearerEvents
                    {
                        // You can expand upon this if you have custom JwtBearerEvents you'd like to handle
                    };
                });
        }
    }
}
