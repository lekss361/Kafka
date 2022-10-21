using API.Services;
using KafkaFlow.Producers;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API.HealthCheck;

public class SampleHealthCheck : IHealthCheck
{
    private readonly IMessagePublisherService publisher;
    private readonly IProducerAccessor _producer;

    public SampleHealthCheck(IMessagePublisherService publisher, IProducerAccessor producer)
    {
        this.publisher = publisher;
        _producer = producer;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var allProduce = _producer.All;
        bool isHealthy = allProduce.Count() > 1;
        //var c= await publisher.PublishMessageAsync("healthy", "debug");

        return isHealthy ? HealthCheckResult.Healthy("healthy") : HealthCheckResult.Unhealthy("error");
    }
}
