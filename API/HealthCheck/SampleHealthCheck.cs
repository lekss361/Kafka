using API.Services;
using KafkaFlow.Consumers;
using KafkaFlow.Producers;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API.HealthCheck;

public class SampleHealthCheck : IHealthCheck
{
    private readonly IConsumerAccessor _consumer;
    private readonly IMessagePublisherService _publisherService;

    public SampleHealthCheck(IConsumerAccessor consumer, IMessagePublisherService publisherService)
    {
        _consumer = consumer;
        _publisherService = publisherService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        bool isHealthy=true;
        var allConsumer = _consumer.All;
        _publisherService.PublishMessageAsync("", "sample-topi");
        foreach (var item in allConsumer)
        {
            if (item.MemberId == "")
            {
                isHealthy = false;
                break;
            }
        }

        return isHealthy ? HealthCheckResult.Healthy("healthy") : HealthCheckResult.Unhealthy("error");
    }
}
