using API.Model;
using Confluent.Kafka;
using KafkaFlow;
using KafkaFlow.Admin.Messages;
using KafkaFlow.Admin;
using KafkaFlow.Producers;
using Newtonsoft.Json;

namespace API.Services;

public class MessagePublisherService : IMessagePublisherService
{
    private readonly IProducerAccessor _producerAccessor;
    private readonly ILogger<MessagePublisherService> _logger;

    public MessagePublisherService(IProducerAccessor producerAccessor, ILogger<MessagePublisherService> logger)
    {
        _producerAccessor = producerAccessor ?? throw new ArgumentNullException(nameof(producerAccessor));
        _logger = logger;
    }

    public Task<DeliveryResult<byte[], byte[]>> PublishMessageAsync<T>(T message, string topic)
   where T : class
    {

        _logger.LogDebug($"topic:{topic}");

        var producer = _producerAccessor.GetProducer(topic);

        if (producer == null)
            throw new ArgumentNullException($"no producer for {typeof(T)}");
        var result = producer.ProduceAsync(topic, message.ToString());
        return result;
    }
}