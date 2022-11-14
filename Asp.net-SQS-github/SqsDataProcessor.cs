using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Asp.net_SQS_github
{
    public class SqsDataProcessor : BackgroundService
    {
        private readonly IConfiguration _configuration;

        public SqsDataProcessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting Background processor");
            var credentials = new BasicAWSCredentials(_configuration["AWS:accessKey"], _configuration["AWS:SecretKey"]);
            var client = new AmazonSQSClient(credentials, RegionEndpoint.EUWest3);

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"getting messages from the queue <{DateTime.Now}>");
                var request = new ReceiveMessageRequest()
                {
                    QueueUrl = _configuration["AWS:queueUrl"],
                    WaitTimeSeconds = 20,
                    VisibilityTimeout = 20
                };

                var response = await client.ReceiveMessageAsync(request);
                foreach (var message in response.Messages)
                {
                    Console.WriteLine(message.Body);
                    if (message.Body.Contains("Exception")) continue;

                    await client.DeleteMessageAsync(
                        _configuration["AWS:queueUrl"],
                        message.ReceiptHandle);
                }
            }
        }
    }
}
