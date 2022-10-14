using API.Model;
using Confluent.Kafka;
using KafkaFlow.Producers;

namespace API;

public class KafkaMessagePublisher : IKafkaMessagePublisher
{
    private readonly IProducerAccessor _producerAccessor;

    public KafkaMessagePublisher(IProducerAccessor producerAccessor)
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

        return producer.ProduceAsync(topic, message.ToString());
    }
}