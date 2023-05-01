using OperationStacked.Communication;
using OperationStacked.Data;
using OperationStacked.Extensions.FactoryExtensions;
using OperationStacked.Extensions.ServiceExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Amazon.Runtime;

// Add the following using statements:
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

var connectionString = RemovePortFromServer(await GetConnectionStringFromParameterStore());

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.Configure<ConnectionStringOptions>(options =>
{
    options.ConnectionString = connectionString;
});

var tokenOptions = config.GetSection("TokenOptions").Get<TokenOptions>();
var signingConfigurations = new SigningConfigurations(tokenOptions.Secret);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var assembly = typeof(Program).Assembly;
builder.Services.AddServices();
builder.Services.AddExerciseStrategy();
builder.Services.Configure<TokenOptions>(config.GetSection("TokenOptions"));
builder.Services.RegisterA2S();
builder.Services.AddSingleton(signingConfigurations);
builder.Services.AddExerciseFactory();
builder.Services.AddHealthChecks();
builder.Services.AddRepositories();
builder.Services.AddCors(options =>
    options.AddPolicy("MyPolicy",
        builder => {
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        }
    )
);

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var region = Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION");
    var userPoolId = Environment.GetEnvironmentVariable("AWS_UserPoolId");
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
        OnTokenValidated = async context =>
        {
            // You can add custom logic here, for example:
            // - Check if the token is revoked
            // - Perform additional user-specific validations
        },
        OnAuthenticationFailed = async context =>
        {
            // You can log the exception for debugging purposes
        }
    };
});

builder.Services.AddDbContext<OperationStackedContext>(options =>
{
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29)));
});

var app = builder.Build();
app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OperationStackedContext>();
    app.UseCors("MyPolicy");
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseAuthentication(); // Add this line
    app.UseAuthorization();
    app.MapControllers();
    await dbContext.Database.EnsureCreatedAsync();
    app.Run();
}


async Task<string> GetConnectionStringFromParameterStore()
{
    var access = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
    var secret = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
    var awsCredentials = new BasicAWSCredentials(secret, access);
    var client = new AmazonSimpleSystemsManagementClient(awsCredentials, Amazon.RegionEndpoint.EUWest2);
    var request = new GetParameterRequest
    {
        Name = "operation-stacked-connection-string",
        WithDecryption = true
    };

    var response = await client.GetParameterAsync(request);
    return response.Parameter.Value;
}

string RemovePortFromServer(string connectionString)
{
    const string port = ":3306";
    var serverKey = "server=";
    var serverStartIndex = connectionString.IndexOf(serverKey) + serverKey.Length;
    var portIndex = connectionString.IndexOf(port, serverStartIndex);

    if (portIndex != -1)
    {
        connectionString = connectionString.Remove(portIndex, port.Length);
    }

    return connectionString;
}

public partial class Program { }

public class ConnectionStringOptions
{
    public string ConnectionString { get; set; }
}

