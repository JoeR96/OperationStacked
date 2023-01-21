using Amazon;
using Amazon.DynamoDBv2;

namespace OperationStacked.Utilities;
public static class DynamoDbUtilities
{
    public static IAmazonDynamoDB CreateAmazonDynamoDb(string serviceUrl)
    {
        var clientConfig = new AmazonDynamoDBConfig {RegionEndpoint = RegionEndpoint.EUWest1};

        if (!string.IsNullOrEmpty(serviceUrl))
        {
            clientConfig.ServiceURL = serviceUrl;
        }

        var dynamoClient = new AmazonDynamoDBClient(clientConfig);

        return dynamoClient;
    }
}