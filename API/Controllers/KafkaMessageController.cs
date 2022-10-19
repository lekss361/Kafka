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
    private ConsumeMassagesKafka _consumeMassagesKafka;

    public KafkaMessageController(IMessagePublisherService publisher, ConsumeMassagesKafka consumeMassagesKafka)
    {
        _publisher = publisher;
        _consumeMassagesKafka = consumeMassagesKafka;
    }

    [HttpPut("AddMessage")]
    public async Task<ActionResult> AddMessage([FromBody] Object message, string topicName = "sample-topic")
    {
        var result = await _publisher.PublishMessageAsync(message, topicName);
        return Ok(result);    
        
    }

    [HttpGet("GetMessages")]
    public async Task<ActionResult<List<ResponseKafkaMessagesModel>>> GetMessages( string topicName = "sample-topic", int printLastMessages= 5)
    {
        return Ok(await _consumeMassagesKafka.PrintLastMessages(printLastMessages));
    }
}