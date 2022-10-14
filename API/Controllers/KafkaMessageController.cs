using Microsoft.AspNetCore.Mvc;
using API.Model;
using API.Extensions;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class KafkaMessageController : ControllerBase
{
    private readonly IKafkaMessagePublisher publisher;

    public KafkaMessageController(IKafkaMessagePublisher publisher)
    {
        this.publisher = publisher;
    }

    [HttpPut("AddMessage")]
    public async Task<ActionResult> AddMessage([FromBody] Object message, string topicName = "sample-topic")
    {
        var result = await publisher.PublishMessageAsync(message, topicName);
        return Ok(result);    
        
    }

    [HttpGet("GetMessages")]
    public async Task<ActionResult<List<ResponseKafkaMessagesModel>>> GetMessages( string topicName = "sample-topic", int printLastMessages= 5)
    {
        return Ok(ConsumMassagesKafka.PrintLastMessages(printLastMessages));
    }
}