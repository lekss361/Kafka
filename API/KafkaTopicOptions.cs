public class KafkaTopicOptions
{
        public IDictionary<string, ProducerOptions> Producers { get; } = new Dictionary<string, ProducerOptions>(StringComparer.OrdinalIgnoreCase);
   
}