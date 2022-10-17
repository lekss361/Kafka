using API.Model;
using Confluent.Kafka;
using KafkaFlow.Producers;
using Newtonsoft.Json;

namespace API.Services;

public class MessagePublisherService : IMessagePublisherService
{
    private readonly IProducerAccessor _producerAccessor;

    public MessagePublisherService(IProducerAccessor producerAccessor)
    {
        _producerAccessor = producerAccessor ?? throw new ArgumentNullException(nameof(producerAccessor));
    }

    public Task<DeliveryResult<byte[], byte[]>> PublishMessageAsync<T>(T message, string topic)
   where T : class
    {

        if (topic == null)
            throw new ArgumentNullException($"no topic for {typeof(T)}");

        var producer = _producerAccessor.GetProducer(topic);

        if (producer == null)
            throw new ArgumentNullException($"no producer for {typeof(T)}");
        var result = producer.ProduceAsync(topic, message.ToString());
        return result;
    }
}