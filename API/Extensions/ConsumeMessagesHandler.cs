using KafkaFlow;
using System.Diagnostics;
using KafkaFlow.TypedHandler;
using API.Controllers;
using Newtonsoft.Json;

namespace API.Extensions
{
    public class ConsumeMessagesHandler : IMessageHandler<string>
    {
        private readonly ILogger<ConsumeMessagesHandler> _logger;

        public ConsumeMessagesHandler(ILogger<ConsumeMessagesHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(IMessageContext context, string message)
        {
            GlobalVariables.responseKafkaMessages.Add(new Model.ResponseKafkaMessagesModel(context.ConsumerContext.Offset, message));
            _logger.LogInformation(JsonConvert.SerializeObject(context));
            Debug.WriteLine(
                "Partition: {0} | Offset: {1} | Message: {2}",
                context.ConsumerContext.Partition,
                context.ConsumerContext.Offset,
                message);

            return Task.CompletedTask;
        }
    }
}
