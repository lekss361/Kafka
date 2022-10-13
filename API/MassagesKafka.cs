using KafkaFlow;
using Microsoft.AspNetCore.SignalR.Protocol;
using System.Collections.Generic;

namespace API
{
     public static class MassagesKafka
    {
        public static List<IConsumerContext> messagesContexts { get; set; } = new List<IConsumerContext>();
        
        public static List<IConsumerContext> PrintLastMessages(int  count)
        {
            List < IConsumerContext > removeMessagesContexts = new List<IConsumerContext>(messagesContexts);
            int v = (removeMessagesContexts.Count() - count) < 0 ? 0 : removeMessagesContexts.Count() - count;
            removeMessagesContexts.RemoveRange(0, v);
            return removeMessagesContexts;
        }
    }
}
