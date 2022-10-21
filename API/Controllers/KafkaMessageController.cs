using Microsoft.AspNetCore.Mvc;
using API.Model;
using API.Serialize;
using API.Services;
using API.Extensions;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class KafkaMessageController : ControllerBase
{
    private readonly IMessagePublisherService _publisher;
    private readonly ILogger<KafkaMessageController> _logger;
    private IConsumeMassagesKafka _consumeMassagesKafka;

    public KafkaMessageController(IMessagePublisherService publisher, ILogger<KafkaMessageController> logger, IConsumeMassagesKafka consumeMassagesKafka)
    {
        _publisher = publisher;
        _consumeMassagesKafka = consumeMassagesKafka;
        _logger = logger;
        _logger.LogDebug(1, "NLog injected into KafkaMessageController");
    }

    [HttpPut("AddMessage")]
    public async Task<ActionResult> AddMessage([FromBody] Object message, string topicName = "sample-topic")
    {


        _logger.LogInformation("Hello, this is the index!");
        var result = await _publisher.PublishMessageAsync(message, topicName);
        _logger.LogInformation(result.Offset.ToString());
        return Ok(result);    
        
    }

    [HttpGet("GetMessages")]
    public async Task<ActionResult<List<ResponseKafkaMessagesModel>>> GetMessages( string topicName = "sample-topic", int printLastMessages= 5)
    {
        return Ok(await _consumeMassagesKafka.PrintLastMessages(printLastMessages));
    }
}