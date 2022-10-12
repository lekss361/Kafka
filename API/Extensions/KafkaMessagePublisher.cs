using API.Model;
using Confluent.Kafka;
using KafkaFlow.Producers;

namespace API.Extensions
{
    public class KafkaMessagePublisher : IKafkaMessagePublisher
    {
        private readonly IProducerAccessor _producerAccessor;

        public KafkaMessagePublisher(IProducerAccessor producerAccessor)
        {
            _producerAccessor = producerAccessor ?? throw new ArgumentNullException(nameof(producerAccessor));
        }

        public Task<DeliveryResult<byte[], byte[]>> PublishMessageAsync<T>(T message, string topic,
            IEnumerable<KeyValuePair<string, string>>? messageHeaders = null)
       where T : class
        {
            var producer = _producerAccessor.GetProducer(topic);

            if (producer == null)
                throw new ArgumentNullException($"no producer for {typeof(T)}");

            return producer.ProduceAsync(topic, message);
        }
    }
}