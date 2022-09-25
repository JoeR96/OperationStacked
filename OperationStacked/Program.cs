
using OperationStacked.Abstractions;
using OperationStacked.Extensions;
using OperationStacked.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var assembly = typeof(Program).Assembly;
//builder.Services.AddSingletonsByConvention(assembly, x => x.Name.EndsWith("Service"));
builder.Services.AddTransient<IExerciseProgressionService,ExerciseProgressionService>();
builder.Services.AddTransient<IExerciseCreationService,ExerciseCreationService>();
builder.Services.AddExerciseFactory();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }
