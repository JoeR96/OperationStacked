using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationStacked.Services.Logger
{
    public interface ICloudWatchLogger
    {
        Task InitializeAsync();
        Task LogMessageAsync(string message);
    }

    public class CloudWatchLogger : ICloudWatchLogger
    {
        private readonly IAmazonCloudWatchLogs _client;
        private readonly string _logGroup;
        private string _logStream;
        private string _nextSequenceToken;
        
        public CloudWatchLogger(string logGroup, RegionEndpoint region)
        {
            _logGroup = logGroup ?? throw new ArgumentNullException(nameof(logGroup));
            _client = new AmazonCloudWatchLogsClient(region ?? RegionEndpoint.EUWest2);
        }

        public async Task InitializeAsync()
        {
            await CreateLogGroupIfNotExistsAsync();
            await CreateLogStreamAsync();
        }

        private async Task CreateLogGroupIfNotExistsAsync()
        {
            try
            {
                var existingLogGroups = await _client.DescribeLogGroupsAsync(new DescribeLogGroupsRequest { LogGroupNamePrefix = _logGroup });
                
                if (!existingLogGroups.LogGroups.Any(x => x.LogGroupName == _logGroup))
                {
                    await _client.CreateLogGroupAsync(new CreateLogGroupRequest { LogGroupName = _logGroup });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating/checking log group: {ex.Message}");
            }
        }

        private async Task CreateLogStreamAsync()
        {
            try
            {
                _logStream = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                await _client.CreateLogStreamAsync(new CreateLogStreamRequest { LogGroupName = _logGroup, LogStreamName = _logStream });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating log stream: {ex.Message}");
            }
        }

        public async Task LogMessageAsync(string message)
        {
            try
            {
                var response = await _client.PutLogEventsAsync(new PutLogEventsRequest
                {
                    LogGroupName = _logGroup,
                    LogStreamName = _logStream,
                    LogEvents = new List<InputLogEvent>
                    {
                        new InputLogEvent { Message = message, Timestamp = DateTime.UtcNow }
                    },
                    SequenceToken = _nextSequenceToken 
                });

                _nextSequenceToken = response.NextSequenceToken;
            }
            catch (InvalidSequenceTokenException ex)
            {
                _nextSequenceToken = ex.ExpectedSequenceToken;
                await LogMessageAsync(message); // Retry with the correct sequence token.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging message: {ex.Message}");
            }
        }
    }
}
