using Confluent.Kafka;
using KafkaFlow.Configuration;

namespace API.Model;

public class KafkaConfigModel
{
    public string[] Brokers { get; set; } = Array.Empty<string>();
    public SecurityInformation SecurityInformation { get; set; } = new();
    public  ProducerConfig ProducerConfig { get; set; } = new();
    public ConsumerConfig ConsumerConfug { get; set; } = new();
    public Dictionary<string, string> ConsumerConfig { get; set; } = new();
    public Dictionary<string, string> ProducerConfigs { get; set; } = new();



}




