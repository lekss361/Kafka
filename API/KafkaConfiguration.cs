using System.Collections.Concurrent;

namespace Ekassir.KafkaFlow.Extensions.Registration
{
    public class KafkaConfiguration
    {
        public string[] Brokers { get; set; } = Array.Empty<string>();

        public string? ClusterName { get; set; }

        public bool? Metrics { get; set; } = true;

        public Dictionary<string, string> ProducerConfig { get; set; } = new();

        public Dictionary<string, string> ConsumerConfig { get; set; } = new();
        
        public Dictionary<string, ConsumerConfiguration> Consumers { get; set; } = new();

        public Dictionary<string, ProducerConfiguration> Producers { get; set; } = new();
        
        public ProduceConfiguration[] Produce { get; set; } = Array.Empty<ProduceConfiguration>();

        public Dictionary<string, string> SchemaRegistry { get; set; } = new();

        private readonly ConcurrentDictionary<string, ProduceConfiguration> _producerByTopic = new();

        // ReSharper disable once InconsistentNaming
        private static WildcardOptions CaseInsensitiveOpts =
            WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant | WildcardOptions.Compiled;

        public ProduceConfiguration GetProduceConfiguration(string topic)
        {
            return _producerByTopic.GetOrAdd(topic,
                s =>
                {
                    var produce = Produce.FirstOrDefault(
                        x => topic.IsWildcardMatch(x.Pattern, CaseInsensitiveOpts));
                    if (produce != null)
                    {
                        return produce;
                    }

                    throw new InvalidOperationException(
                        $"Produce configuration for `{topic}` not found");
                });
        }
    }

    public class ConsumerConfiguration
    {
        public string Topic { get; set; } = "";

        public int BufferSize { get; set; } = 100;

        public int WorkersCount { get; set; } = 1;

        public Dictionary<string, string> Config { get; set; } = new();
        
        public bool Logging { get; set; } = true;

        public bool LoggerScopes { get; set; } = true;

        public bool? Metrics { get; set; }

        public bool UseDeadLetterQueue { get; set; }

        public bool UseCorrelationId { get; set; } = true;

        public bool UseHeaderPropagation { get; set; } = true;
    }

    public class ProducerConfiguration
    {
        public SchemaType Schema { get; set; }

        public Dictionary<string, string> Config { get; set; } = new();
        
        public bool Logging { get; set; } = true;

        public bool? Metrics { get; set; }

        public bool UseCorrelationId { get; set; } = true;

        public bool UseHeaderPropagation { get; set; } = true;
    }

    public enum SchemaType
    {
        Avro,
        None
    }

    public class ProduceConfiguration
    {
        public string Pattern { get; set; } = "";

        public string? Topic { get; set; }

        public string Producer { get; set; } = "";
    }

    public static class PipelineConfigurationExceptions
    {
        public static ConsumerConfiguration GetConsumerConfig(this KafkaConfiguration self, string alias)
        {
            if (!self.Consumers.TryGetValue(alias, out var configuration))
            {
                throw new InvalidOperationException(
                    $"Consumer `{alias}` not found on configuration");
            }

            configuration =
                new ConsumerConfiguration()
                {
                    Topic = configuration.Topic,
                    BufferSize = configuration.BufferSize,
                    WorkersCount = configuration.WorkersCount,
                    Config = configuration.Config.ToDictionary(entry => entry.Key,
                        entry => entry.Value),
                    Logging = configuration.Logging,
                    LoggerScopes = configuration.LoggerScopes,
                    Metrics = configuration.Metrics ?? self.Metrics,
                    UseDeadLetterQueue = configuration.UseDeadLetterQueue,
                    UseCorrelationId = configuration.UseCorrelationId,
                    UseHeaderPropagation = configuration.UseHeaderPropagation
                };

            foreach (var kvp in self.ConsumerConfig)
            {
                if (!configuration.Config.ContainsKey(kvp.Key))
                {
                    configuration.Config.Add(kvp.Key, kvp.Value);
                }
            }

            return configuration;
        }

        public static ProducerConfiguration GetProducerConfig(this KafkaConfiguration self, string alias)
        {
            if (!self.Producers.TryGetValue(alias, out var configuration))
            {
                throw new InvalidOperationException(
                    $"Producer `{alias}` not found on configuration");
            }

            configuration =
                new ProducerConfiguration()
                {
                    Schema = configuration.Schema,
                    Config = configuration.Config.ToDictionary(entry => entry.Key,
                        entry => entry.Value),
                    Logging = configuration.Logging,
                    Metrics = configuration.Metrics ?? self.Metrics,
                    UseCorrelationId = configuration.UseCorrelationId,
                    UseHeaderPropagation = configuration.UseHeaderPropagation
                };

            foreach (var kvp in self.ProducerConfig)
            {
                if (!configuration.Config.ContainsKey(kvp.Key))
                {
                    configuration.Config.Add(kvp.Key, kvp.Value);
                }
            }

            return configuration;
        }
    }
}