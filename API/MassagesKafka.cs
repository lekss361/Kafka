using KafkaFlow;

namespace API
{
     public static class MassagesKafka
    {
        public static List<IConsumerContext> messagesContexts { get; set; } = new List<IConsumerContext>();    
    }
}
