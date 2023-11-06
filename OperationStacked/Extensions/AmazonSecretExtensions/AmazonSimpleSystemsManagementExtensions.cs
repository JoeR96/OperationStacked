using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Extensions.Options;
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
    
    public static async Task<IServiceCollection> ConfigureConnectionStringFromOptionsAsync(this IServiceCollection services)
    {
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var awsOptions = serviceProvider.GetRequiredService<IOptions<AWSOptions>>().Value;

            var awsCredentials = new BasicAWSCredentials(awsOptions.AccessKeyId, awsOptions.SecretAccessKey);
            var ssmClient = new AmazonSimpleSystemsManagementClient(awsCredentials, Amazon.RegionEndpoint.EUWest2);  // You can also refactor to use the Region from AWSOptions

            var connectionString = RemovePortFromServer(await ssmClient.GetConnectionStringAsync("operation-stacked-connection-string"));

            services.Configure<ConnectionStringOptions>(options =>
            {
                options.ConnectionString = connectionString;
            });
        }
    
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