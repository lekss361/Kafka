using KafkaFlow;
using System.Diagnostics;
using KafkaFlow.TypedHandler;

namespace API.Extensions
{
    public class ConsumMessagesHandler : IMessageHandler<string>
    {
        public Task Handle(IMessageContext context, string message)
        {
            ConsumMassagesKafka.messagesContexts.Add(new Model.ResponseKafkaMessagesModel(context.ConsumerContext.Offset, message));

            Debug.WriteLine(
                "Partition: {0} | Offset: {1} | Message: {2}",
                context.ConsumerContext.Partition,
                context.ConsumerContext.Offset,
                message);

            return Task.CompletedTask;
        }
    }
}
