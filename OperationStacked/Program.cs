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

    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;

});
builder.Services.AddServices();
builder.Services.AddHealthChecks();
builder.Services.AddRepositories();

builder.Services.AddCustomCorsPolicy();

builder.Services.AddCognitoAuthentication();
builder.Services.AddSingleton<ICloudWatchLogger>(sp => new CloudWatchLogger("operation-stacked", RegionEndpoint.EUWest2));
builder.Services.AddOperationStackedContext();


var app = builder.Build();
app.MapHealthChecks("/health");

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<OperationStackedContext>();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Operation Stacked Workout V1.2"));
app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();
await dbContext.Database.EnsureCreatedAsync();
app.Run();

public partial class Program { }
