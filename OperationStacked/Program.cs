using System.Text.Json.Serialization;
using Amazon;
using Microsoft.OpenApi.Models;
using OperationStacked.Data;
using OperationStacked.Extensions.AuthenticationExtensions;
using OperationStacked.Extensions.CorsExtensions;
using OperationStacked.Extensions.ServiceExtensions;
using OperationStacked.Services.Logger;

var builder = WebApplication.CreateBuilder(args);
await builder.Services.SetAWSOptionsAsync();
await builder.Services.ConfigureConnectionStringFromOptionsAsync();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
    })
    ;


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;

    // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
builder.Services.AddServices();
builder.Services.AddHealthChecks();
builder.Services.AddRepositories();

builder.Services.AddCustomCorsPolicy();

builder.Services.AddCognitoAuthentication();
builder.Services.AddSingleton<ICloudWatchLogger>(sp => new CloudWatchLogger("operation-stacked", RegionEndpoint.EUWest2));
builder.Services.AddOperationStackedContext();


var app = builder.Build();
bool isLocalDev = app.Environment.IsDevelopment();

app.MapHealthChecks("/health");

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<OperationStackedContext>();
app.UseCors("MyPolicy");

app.UseSwaggerUI(c =>
{
    // Correct Swagger JSON path based on whether the app is running in local development or behind the proxy
    var swaggerJsonBasePath = isLocalDev ? "/" : "/workout/";
    Console.Write(swaggerJsonBasePath);
    c.SwaggerEndpoint($"{swaggerJsonBasePath}swagger/v1/swagger.json", "Operation Stacked Workout V1");

    // Set the route prefix for accessing the Swagger UI
    c.RoutePrefix = isLocalDev ? "swagger" : "workout/swagger";
});

app.UsePathBase("/workout/");

app.UseSwagger(c =>
{
    c.RouteTemplate = isLocalDev ? "swagger/{documentName}/swagger.json" : "workout/swagger/{documentName}/swagger.json";
});


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});await dbContext.Database.EnsureCreatedAsync();
app.Run();

public partial class Program { }
