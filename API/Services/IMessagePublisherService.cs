using Confluent.Kafka;

namespace API.Services
{
    public interface IMessagePublisherService
    {
        Task<DeliveryResult<byte[], byte[]>> PublishMessageAsync<T>(T message, string topic) where T : class;
    }
}