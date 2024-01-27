using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OperationStacked.Data;
using OperationStacked.Extensions.AuthenticationExtensions;
using OperationStacked.Extensions.CorsExtensions;
using OperationStacked.Extensions.ServiceExtensions;
using OperationStacked.Services.Logger;

var builder = WebApplication.CreateBuilder(args);

// Determine if the environment is local development
bool isLocalDev = builder.Environment.IsDevelopment();

string appVersion = "1.0.0"; // Default version

if (isLocalDev)
{
    var s3Client = new AmazonS3Client(RegionEndpoint.EUWest2);
    appVersion = await GetLatestVersionFromS3Async(s3Client, "operation-stacked-workout-version");
}
else
{
    appVersion = Environment.GetEnvironmentVariable("APP_VERSION") ?? appVersion;
}

await builder.Services.SetAWSOptionsAsync();
await builder.Services.ConfigureConnectionStringFromOptionsAsync();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Operation Stacked Workout",
        Version = appVersion
    });
    c.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
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
app.UseCors("MyPolicy");
app.UseSwaggerUI(c =>
{
    var swaggerJsonBasePath = "/workout/";
    if (isLocalDev)
    {
        swaggerJsonBasePath = "/";
    }

    c.SwaggerEndpoint($"{swaggerJsonBasePath}swagger/v1/swagger.json", "Operation Stacked Workout");
    c.RoutePrefix = "swagger";
});

app.UsePathBase("/workout");

app.UseSwagger();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<OperationStackedContext>();
await dbContext.Database.EnsureCreatedAsync();
app.Run();

 async Task<string> GetLatestVersionFromS3Async(AmazonS3Client s3Client, string bucketName)
{
    try
    {
        // List objects in the specified S3 bucket
        var listObjectsRequest = new ListObjectsV2Request
        {
            BucketName = bucketName
        };

        ListObjectsV2Response response = await s3Client.ListObjectsV2Async(listObjectsRequest);
        var objects = response.S3Objects;

        // Assuming the version files are named in a sortable manner
        // e.g., "version_1", "version_2" and so on
        var latestObject = objects
            .OrderByDescending(s3Object => s3Object.Key) // Sort by key name in descending order
            .FirstOrDefault(); // Get the first object, which is the latest one

        return latestObject?.Key; // Return the key of the latest object or null if there are no objects
    }
    catch (AmazonS3Exception e)
    {
        // Handle AWS S3 exception
        Console.WriteLine("Error encountered on server. Message:'{0}' when listing objects", e.Message);
    }
    catch (Exception e)
    {
        // Handle other exceptions
        Console.WriteLine("Unknown encountered on server. Message:'{0}' when listing objects", e.Message);
    }

    return null;
}
