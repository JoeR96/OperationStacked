using System;
using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.RuntimeSupport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OperationStacked.Communication;
using OperationStacked.Data;
using OperationStacked.Extensions.FactoryExtensions;
using OperationStacked.Extensions.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME")))
{
    // Not running in Lambda, run the application as usual.
    ConfigureServices(builder);
    ConfigureApp(builder.Build()).Run();
}
else
{
    // Running in Lambda, start the Lambda bootstrap process.
    var lambdaEntryPoint = new LambdaEntryPoint();
    var functionHandler = (Func<APIGatewayProxyRequest, ILambdaContext, Task<APIGatewayProxyResponse>>)(lambdaEntryPoint.FunctionHandlerAsync);
    using var handlerWrapper = HandlerWrapper.GetHandlerWrapper(functionHandler, new JsonSerializer());
    using var bootstrap = new LambdaBootstrap(handlerWrapper);
    await bootstrap.RunAsync();
}

void ConfigureServices(WebApplicationBuilder builder)
{
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
}

WebApplication ConfigureApp(WebApplication app)
{
    app.MapHealthChecks("/health");

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<OperationStackedContext>();
        app.UseCors("MyPolicy");
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.MapControllers();
        dbContext.Database.EnsureCreatedAsync().Wait();
    }

    return app;
}

public partial class Program { }

public class LambdaEntryPoint : APIGatewayProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder.ConfigureServices(ConfigureServices);
        builder.Configure(app =>
        {
            ConfigureApp(app);
        });
    }
}
