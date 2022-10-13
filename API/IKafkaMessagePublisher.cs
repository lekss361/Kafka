using Confluent.Kafka;

namespace API
{
    public interface IKafkaMessagePublisher
    {
        Task<DeliveryResult<byte[], byte[]>> PublishMessageAsync<T>(T message, string topic) where T : class;
    }
}