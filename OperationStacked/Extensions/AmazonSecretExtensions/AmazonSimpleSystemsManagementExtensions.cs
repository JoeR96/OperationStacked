using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using OperationStacked.Options;

public static class AmazonSimpleSystemsManagementExtensions
{
    public static async Task<string> GetConnectionStringAsync(this AmazonSimpleSystemsManagementClient client, string parameterName)
    {
        var request = new GetParameterRequest
        {
            Name = parameterName,
            WithDecryption = true
        };
        var response = await client.GetParameterAsync(request);
        return response.Parameter.Value;
    }
    
    public static IServiceCollection ConfigureConnectionStringFromOptions(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("OperationStackedConnectionString");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Database connection string not found.");
        }

        services.Configure<ConnectionStringOptions>(options =>
        {
            options.ConnectionString = connectionString;
        });

        return services;
    }

    private static string RemovePortFromServer(string connectionString)
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
}
