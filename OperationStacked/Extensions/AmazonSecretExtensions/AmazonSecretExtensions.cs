using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using OperationStacked.Options;

public static class AmazonSecretsManagerExtensions
{
    public static async Task<string> GetSecretAsync(this AmazonSecretsManagerClient client, string secretId)
    {
        var response = await client.GetSecretValueAsync(new GetSecretValueRequest { SecretId = secretId });
        var secretJson = response.SecretString;
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(secretJson).FirstOrDefault().Value;
    }
    
    public static async Task SetEnvironmentVariableFromSecretAsync(this AmazonSecretsManagerClient client, string secretId, string envVarName)
    {
        var value = await client.GetSecretAsync(secretId);
        Environment.SetEnvironmentVariable(envVarName, value);
    }
    
    public static async Task<IServiceCollection> SetAWSOptionsAsync(this IServiceCollection services)
    { 
        var client = new AmazonSecretsManagerClient(Amazon.RegionEndpoint.EUWest2);

        var config = new AWSOptions
        {        
            // AccessKeyId = await client.GetSecretAsync("AWS_ACCESS_KEY_ID"),
            // SecretAccessKey = await client.GetSecretAsync("AWS_SECRET_ACCESS_KEY"),
            // Region = Amazon.RegionEndpoint.EUWest2.ToString(),
            // UserPoolId = await client.GetSecretAsync("AWS_UserPoolId")
        };

        // Configure AWSOptions for dependency injection
        services.Configure<AWSOptions>(options =>
        {
            options.AccessKeyId = config.AccessKeyId;
            options.SecretAccessKey = config.SecretAccessKey;
            options.Region = config.Region;
            options.UserPoolId = config.UserPoolId;
        });

        return services;
    }

 
}
