using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
// Add the following using statements:
using Newtonsoft.Json;
using OperationStacked.Communication;
using OperationStacked.Data;
using OperationStacked.Extensions.FactoryExtensions;
using OperationStacked.Extensions.ServiceExtensions;
using OperationStacked.Factories;
using OperationStacked.Repositories;
using OperationStacked.Services.Logger;
using OperationStacked.Services.RecipeService;

Console.WriteLine(Environment.GetEnvironmentVariables());
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
builder.Services.AddServices();
builder.Services.Configure<TokenOptions>(config.GetSection("TokenOptions"));
builder.Services.RegisterA2S();
builder.Services.AddSingleton(signingConfigurations);
builder.Services.AddExerciseFactory();
builder.Services.AddHealthChecks();
builder.Services.AddRepositories();
builder.Services.AddScoped<LinearProgressionService>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddCors(options =>
    options.AddPolicy("MyPolicy",
        builder => {
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        }
    )
);
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

await ConfigureSecret();

async Task ConfigureSecret()
{
    var secret = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
    var access = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
    var awsCredentials = new BasicAWSCredentials(access, secret);
    var clientsecret = new AmazonSecretsManagerClient(awsCredentials,Amazon.RegionEndpoint.EUWest2);

    var request1 = new GetSecretValueRequest
    {
        SecretId = "AWS_DEFAULT_REGION"
    };

    var response1 = await clientsecret.GetSecretValueAsync(request1);

    var secretJson = response1.SecretString;

    var secret1 = JsonConvert.DeserializeObject<Dictionary<string, string>>(secretJson);

    var request2 = new GetSecretValueRequest
    {
        SecretId = "AWS_UserPoolId"
    };

    var response2 = await clientsecret.GetSecretValueAsync(request2);

    var secretJson2 = response2.SecretString;

    var secret2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(secretJson2);


    Environment.SetEnvironmentVariable("AWS_DEFAULT_REGION", secret1.FirstOrDefault().Value);
    Environment.SetEnvironmentVariable("AWS_UserPoolId", secret2.FirstOrDefault().Value);
}

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
     
    };
});
builder.Services.AddSingleton<ICloudWatchLogger>(sp => new CloudWatchLogger("operation-stacked", RegionEndpoint.EUWest2));

builder.Services.AddDbContext<OperationStackedContext>(options =>
{
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29)));
},ServiceLifetime.Transient);
builder.Services.AddDbContextFactory<OperationStackedContext>(options =>
{
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29)));
},ServiceLifetime.Transient);

var app = builder.Build();
app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OperationStackedContext>();
    app.UseCors("MyPolicy");
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Operation Stacked Workout V1.2"));
    app.UseHttpsRedirection();
    app.UseAuthentication(); 
    app.UseAuthorization();
    app.MapControllers();
    await dbContext.Database.EnsureCreatedAsync();
    app.Run();
}


async Task<string> GetConnectionStringFromParameterStore()
{
    var clientsecret = new AmazonSecretsManagerClient(Amazon.RegionEndpoint.EUWest2);

    var request1 = new GetSecretValueRequest
    {
        SecretId = "AWS_ACCESS_KEY_ID"
    };

    var response1 = await clientsecret.GetSecretValueAsync(request1);

    var secretJson = response1.SecretString;

    var secret1 = JsonConvert.DeserializeObject<Dictionary<string, string>>(secretJson);

    var request2 = new GetSecretValueRequest
    {
        SecretId = "AWS_SECRET_ACCESS_KEY"
    };

    var response2 = await clientsecret.GetSecretValueAsync(request2);

    var secretJson2 = response2.SecretString;

    var secret2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(secretJson2);


    Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", secret1.FirstOrDefault().Value);
    Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", secret2.FirstOrDefault().Value);
    var secret = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
    var access = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
    var awsCredentials = new BasicAWSCredentials(access, secret);
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

