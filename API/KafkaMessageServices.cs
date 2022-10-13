using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    public class KafkaMessageServices : IKafkaMessageServices
    {
        private readonly IKafkaMessageServices publisher;

        public KafkaMessageServices(IKafkaMessageServices publisher)
        {
            this.publisher = publisher;
        }

        public async Task<DeliveryResult<byte[], byte[]>> AddMessage(string message, string topicName = "sample-topic")
        {
            var c = await publisher.AddMessage(message, topicName);
            return c;

        }

    }
}
