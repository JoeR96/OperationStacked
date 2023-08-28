using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;

namespace OperationStacked.Services.Logger;

public class CloudWatchLogger
{
    private IAmazonCloudWatchLogs _client;
    private string _logGroup = "operation-stacked";
    private string _logStream;

    private CloudWatchLogger( )
    {
        _client = new AmazonCloudWatchLogsClient();
    }

    public static async Task<CloudWatchLogger> GetLoggerAsync()
    {
        var logger = new CloudWatchLogger();

        // Create a log group for our logger
        await logger.CreateLogGroupAsync();

        // Create a log stream
        await logger.CreateLogStreamAsync();

        return logger;
    }

    private async Task CreateLogGroupAsync()
    {
        var existingLogGroups = await _client.DescribeLogGroupsAsync();
        if (existingLogGroups.LogGroups.Any(x => x.LogGroupName == _logGroup))
            return;

        _ = await _client.CreateLogGroupAsync(new CreateLogGroupRequest()
        {
            LogGroupName = _logGroup
        });
    }
    
    private async Task CreateLogStreamAsync()
    {
        _logStream = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

        _ = await _client.CreateLogStreamAsync(new CreateLogStreamRequest()
        {
            LogGroupName = _logGroup,
            LogStreamName = _logStream
        });
    }
    
    public async Task LogMessageAsync(string message)
    {
        _ = await _client.PutLogEventsAsync(new PutLogEventsRequest()
        {
            LogGroupName = _logGroup,
            LogStreamName = _logStream,
            LogEvents = new List<InputLogEvent>()
            {
                new InputLogEvent()
                {
                    Message = message,
                    Timestamp = DateTime.UtcNow
                }
            }
        });
    }
}