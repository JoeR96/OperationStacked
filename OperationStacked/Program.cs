using OperationStacked.Communication;
using OperationStacked.Data;
using OperationStacked.Extensions.FactoryExtensions;
using OperationStacked.Extensions.ServiceExtensions;
using Microsoft.EntityFrameworkCore;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

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



await using var app = builder.Build();

// Local async function to fetch the connection string and configure the application
async Task ConfigureApp()
{
    var connectionString = await GetConnectionStringFromParameterStore();

    builder.Services.AddDbContext<OperationStackedContext>(options =>
    {
        options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29)));
    });

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
    }
}

await ConfigureApp(); // Call the local async function
app.Run();

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

    //return "server=terraform-20230416162907179900000002.cat7knapsqxr.eu-west-1.rds.amazonaws.com:3306;Port=3306;Database=OperationStacked;User Id=operationstacked;Password=your_secure_password;";
}


public partial class Program { }

