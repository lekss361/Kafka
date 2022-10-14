using API.Model;
using Newtonsoft.Json;

namespace API;

 public static class ConsumMassagesKafka
{
    public static List<ResponseKafkaMessagesModel> messagesContexts { get; set; } = new List<ResponseKafkaMessagesModel>();
    
    public static string PrintLastMessages(int  count)
    {
        List <ResponseKafkaMessagesModel> removeMessagesContexts = new List<ResponseKafkaMessagesModel>(messagesContexts);

        int remainderMessagesKafka = (removeMessagesContexts.Count() - count) < 0 ? 0 : removeMessagesContexts.Count() - count;
        removeMessagesContexts.RemoveRange(0, remainderMessagesKafka);
        return JsonConvert.SerializeObject(removeMessagesContexts, Formatting.Indented);
    }
}
