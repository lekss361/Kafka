using API.Model;
using API.Serialize;

namespace API.Extensions;

public class ConsumeMassagesKafka : IConsumeMassagesKafka
{
    private readonly object ReadWriteLock = new();

    public Task<string> PrintLastMessages(int count)
    {
        List<ResponseKafkaMessagesModel> removeMessages = new List<ResponseKafkaMessagesModel>();
        if (count < 0)
            throw new ArgumentOutOfRangeException("Count<0");

        CustomSerializer serializer = new CustomSerializer();

        lock (ReadWriteLock)
        {
            removeMessages.AddRange(GlobalVariables.responseKafkaMessages);

            int remainderMessagesKafka = removeMessages.Count - count < 0 ? 0 : removeMessages.Count - count;
            removeMessages.RemoveRange(0, remainderMessagesKafka);
        }
        return Task.FromResult(serializer.JsonConvertSerilize(removeMessages).Result);
    }
}
