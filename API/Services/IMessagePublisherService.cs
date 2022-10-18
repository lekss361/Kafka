using Confluent.Kafka;
using KafkaFlow;

namespace API.Services
{
    public interface IMessagePublisherService
    {
        Task<IEnumerable<IMessageProducer>> GetAllConfigureProduce();
        Task<DeliveryResult<byte[], byte[]>> PublishMessageAsync<T>(T message, string topic) where T : class;
    }
}