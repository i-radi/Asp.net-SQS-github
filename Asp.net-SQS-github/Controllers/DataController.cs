using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Asp.net_SQS_github.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;
        private readonly IConfiguration _configuration;

        public DataController(ILogger<DataController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task Post(SqsData data)
        {
            var credentials = new BasicAWSCredentials(_configuration["AWS:accessKey"], _configuration["AWS:SecretKey"]);
            var client = new AmazonSQSClient(credentials, RegionEndpoint.EUWest3);

            var request = new SendMessageRequest()
            {
                QueueUrl = _configuration["AWS:queueUrl"],
                MessageBody = JsonSerializer.Serialize(data),
                //DelaySeconds = 20,
            };
            _ = await client.SendMessageAsync(request);
        }
    }
}