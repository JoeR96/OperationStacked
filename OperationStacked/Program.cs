
using Microsoft.Extensions.Configuration;
using OperationStacked.Abstractions;
using OperationStacked.Communication;
using OperationStacked.Data;
using OperationStacked.Extensions;
using OperationStacked.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var assembly = typeof(Program).Assembly;
//builder.Services.AddSingletonsByConvention(assembly, x => x.Name.EndsWith("Service"));
builder.Services.AddServices();
builder.Services.Configure<TokenOptions>(config.GetSection("TokenOptions"));
var tokenOptions = config.GetSection("TokenOptions").Get<TokenOptions>();

var signingConfigurations = new SigningConfigurations(tokenOptions.Secret);
builder.Services.AddSingleton(signingConfigurations);
builder.Services.AddExerciseFactory();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OperationStackedContext>();
    // use context
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.MapControllers();
    await dbContext.Database.EnsureCreatedAsync();
    app.Run();
}
// Configure the HTTP request pipeline.


internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }
