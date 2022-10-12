using Microsoft.AspNetCore.Mvc;
using API.Model;
using API.Extensions;
using Confluent.Kafka;
using System.Text.Json;
using KafkaFlow;
using Newtonsoft.Json;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class KafkaMessage : ControllerBase
{
    private readonly IKafkaMessagePublisher publisher;

    public KafkaMessage(IKafkaMessagePublisher publisher)
    {
        this.publisher = publisher;
    }

    [HttpPut("AddMessage")]
    public async Task<ActionResult> AddMessage([FromBody] string message, string topicName = "sample-topic")
    {
         var c = await publisher.PublishMessageAsync(message, topicName);
        return Ok(c);    
        
    }

    [HttpGet("GetMessages")]
    public async Task<ActionResult<List<IConsumerContext>>> GetMessages( string topicName = "sample-topic", int printLastMessages= 5)
    {
        MassagesKafka.PrintLastMessages(printLastMessages);
        return Ok(JsonConvert.SerializeObject(MassagesKafka.messagesContexts, Formatting.Indented));
        

    }
}