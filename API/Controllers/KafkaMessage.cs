using Microsoft.AspNetCore.Mvc;
using API.Model;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class KafkaMessage : ControllerBase
{
    

    [HttpPut("AddMessage")]
    public async Task<ActionResult> AddMessage([FromBody] string message, string topicName = "sample_topia")
    {
        //await new KafkaProducer().AddMessageProducer(topicName, new MessageModel { Data = message });
        return Ok();    
        
    }

    [HttpGet("GetMessages")]
    public async Task<ActionResult> GetMessages( string topicName = "sample_topia")
    {
        
        var messages= await new KafkaConsumer<MessageModel>().GetMessageToKafka(topicName);
        return Ok(messages);

    }
}