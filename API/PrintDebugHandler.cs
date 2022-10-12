using API.Model;
using KafkaFlow;
using System.Diagnostics;
using KafkaFlow.TypedHandler;
using KafkaFlow.Consumers;

namespace API
{
    public class PrintDebugHandler: IMessageHandler<string>
    {
        private readonly IConsumerAccessor _consumerAccessor;

        public Task Handle(IMessageContext context, string message)
        {
            MassagesKafka.messagesContexts.Add(context.ConsumerContext);

            Debug.WriteLine(
                "Partition: {0} | Offset: {1} | Message: {2}",
                context.ConsumerContext.Partition,
                context.ConsumerContext.Offset,
                message);

            return Task.CompletedTask;
        }
    }
}
