using Microsoft.AspNetCore.Mvc;
using API.Model;
using API.Serialize;
using API.Services;
using API.Extensions;
using System.Net;
using System;
using System.Diagnostics;
using Microsoft.Rest;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using System.Text;

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
    }

    [HttpPut("AddMessage")]
    public async Task<ActionResult> AddMessage([FromBody] Object message, string topicName = "sample-topic")
    {
        _logger.LogInformation($"Headers:{JsonConvert.SerializeObject(HttpContext.Request.Headers, Formatting.Indented)}");
       
        var result = await _publisher.PublishMessageAsync(message, topicName);
       
        _logger.LogInformation($"Response: Offset:{result.Offset} KeyMessage:{Encoding.UTF8.GetString(result.Key, 0, result.Key.Length - 1)} " +
            $"Response Endpoint: {JsonConvert.SerializeObject(result,Formatting.Indented)}");
        return Ok(result);
    }

    [HttpGet("GetMessages")]
    public async Task<ActionResult<List<ResponseKafkaMessagesModel>>> GetMessages( string topicName = "sample-topic", int printLastMessages= 5)
    {
        _logger.LogInformation($"Headers:{JsonConvert.SerializeObject(HttpContext.Request.Headers, Formatting.Indented)}");

        var result = await _consumeMassagesKafka.PrintLastMessages(printLastMessages);

        _logger.LogInformation($"Response Endpoint: {JsonConvert.SerializeObject(result, Formatting.Indented)}");
        return Ok(result);
    }
}