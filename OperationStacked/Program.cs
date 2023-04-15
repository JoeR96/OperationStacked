using OperationStacked.Communication;
using OperationStacked.Data;
using OperationStacked.Extensions.FactoryExtensions;
using OperationStacked.Extensions.ServiceExtensions;
using Microsoft.EntityFrameworkCore;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

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
    var connectionString = GetConnectionStringFromParameterStore().Result;
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
    var client = new AmazonSimpleSystemsManagementClient(Amazon.RegionEndpoint.EUWest1);
    var request = new GetParameterRequest
    {
        Name = "/operationstacked/connectionstring/",
        WithDecryption = true
    };

    var response = await client.GetParameterAsync(request);
    return response.Parameter.Value;
}


public partial class Program { }

