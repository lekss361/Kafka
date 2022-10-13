using Confluent.Kafka;

namespace API
{
    public interface IKafkaMessageServices
    {
        Task<DeliveryResult<byte[], byte[]>> AddMessage(string message, string topicName = "sample-topic");
    }
}