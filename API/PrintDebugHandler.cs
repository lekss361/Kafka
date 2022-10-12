using API.Model;
using KafkaFlow;
using System.Diagnostics;
using KafkaFlow.TypedHandler;

namespace API
{
    public class PrintDebugHandler: IMessageHandler<TestMessage>
    {
        public Task Handle(IMessageContext context, TestMessage message)
        {
            Debug.WriteLine(
                "Partition: {0} | Offset: {1} | Message: {2}",
                context.ConsumerContext.Partition,
                context.ConsumerContext.Offset,
                message.Text);

            return Task.CompletedTask;
        }
    }
}
