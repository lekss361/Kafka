using KafkaFlow;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace API
{
     public static class MassagesKafka
    {
        public static List<IConsumerContext> messagesContexts { get; set; } = new List<IConsumerContext>();
        
        public static void PrintLastMessages(int  count)
        {
            var x = messagesContexts.Count();
            int v = (x - count) < 0 ? v = 0 : v = x - count;
            MassagesKafka.messagesContexts.RemoveRange(0, v);
        }
    }
}
