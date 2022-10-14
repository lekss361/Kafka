using API.Model;
using Newtonsoft.Json;

namespace API;

 public static class ConsumMassagesKafka
{
    public static List<OutKafkaMessageModel> messagesContexts { get; set; } = new List<OutKafkaMessageModel>();
    
    public static string PrintLastMessages(int  count)
    {
        List <OutKafkaMessageModel> removeMessagesContexts = new List<OutKafkaMessageModel>(messagesContexts);
        int v = (removeMessagesContexts.Count() - count) < 0 ? 0 : removeMessagesContexts.Count() - count;
        removeMessagesContexts.RemoveRange(0, v);
        return JsonConvert.SerializeObject(removeMessagesContexts, Formatting.Indented);
    }
}
