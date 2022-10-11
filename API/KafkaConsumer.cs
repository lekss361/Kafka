using API.Extensions;
using API.Model;
using API.Serialize;
using Confluent.Kafka;
using System.Diagnostics;

namespace API;

public class KafkaConsumer<T>
{
    List<string> messages = new List<string>();

    public async Task<List<string>> GetMessageToKafka(string topicName)
    {
        var serviceCollection = new ServiceCollection();
        var readerAppSetting = new ReaderAppsetting();
        var v = readerAppSetting.GetConsumerConfig().ConsumerConfig;

        using (var consumer = new ConsumerBuilder<Ignore, T>(readerAppSetting.GetConsumerConfig().ConsumerConfig)
        .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
        .SetStatisticsHandler((_, json) => Console.WriteLine($"Statistics: {json}"))
        .SetValueDeserializer(deserializer: new CustomValueDeserializer<T>())
        .Build())
        {
            consumer.Subscribe(topicName);

            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(10000);
                            if (consumeResult == null)
                            {
                                break;
                            }
                            if (consumeResult.Message.Value is MessageModel result)
                            {
                                messages.Add(Convert.ToString(result));
                            }
                        }
                        catch (ConsumeException e)
                        {
                            Debug.WriteLine($"Consume error: {e.Error.Reason}");
                        }
                    }
                });
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Closing consumer.");
                consumer.Close();
                return messages;
            }
            return messages;
        }
    }
}
