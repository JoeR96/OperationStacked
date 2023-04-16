using OperationStacked.Communication;
using OperationStacked.Data;
using OperationStacked.Extensions.FactoryExtensions;
using OperationStacked.Extensions.ServiceExtensions;
using Microsoft.EntityFrameworkCore;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Amazon.Runtime;
var connectionString = await GetConnectionStringFromParameterStore();

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var connectionStrings = Environment.GetEnvironmentVariables();
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
    app.MapControllers();
    await dbContext.Database.EnsureCreatedAsync();
    app.Run();
}



async Task<string> GetConnectionStringFromParameterStore()
{
    var awsCredentials = new BasicAWSCredentials("AKIARSRO64ZCHOPL376V", "VI28IOUpT5HMrb1k6R2ynSTjC/uhQaqMspK/7v5z");
    var client = new AmazonSimpleSystemsManagementClient(awsCredentials, Amazon.RegionEndpoint.EUWest1);
    var request = new GetParameterRequest
    {
        Name = "operationstacked-connectionstring",
        WithDecryption = true
    };

    var response = await client.GetParameterAsync(request);
    return response.Parameter.Value;
}


public partial class Program { }

public class ConnectionStringOptions
{
    public string ConnectionString { get; set; }
}
