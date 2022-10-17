using API.Model;
using API.Serialize;
using Newtonsoft.Json;

namespace API.Extensions;

public static class ConsumMassagesKafka
{
    public static List<ResponseKafkaMessagesModel> messagesContexts { get; set; } = new List<ResponseKafkaMessagesModel>();

    public static async Task<string> PrintLastMessages(int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException("Count<0");

        CustomSerializer serializer = new CustomSerializer();
        List<ResponseKafkaMessagesModel> removeMessagesContexts = new List<ResponseKafkaMessagesModel>(messagesContexts);

        int remainderMessagesKafka = removeMessagesContexts.Count() - count < 0 ? 0 : removeMessagesContexts.Count() - count;
        removeMessagesContexts.RemoveRange(0, remainderMessagesKafka);
        return serializer.JsonConvertSerilize(removeMessagesContexts).Result;
    }
}
