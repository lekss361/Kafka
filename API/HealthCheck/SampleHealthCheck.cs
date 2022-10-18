using API.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API.HealthCheck;

public class SampleHealthCheck : IHealthCheck
{
    private readonly IMessagePublisherService publisher;

    public SampleHealthCheck(IMessagePublisherService publisher)
    {
        this.publisher = publisher;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        bool isHealthy = await IDataBaseConnectAsync();
        var x = publisher.PublishMessageAsync("","");
        return isHealthy? HealthCheckResult.Healthy("conn"):HealthCheckResult.Unhealthy("error");
    }

    private Task<bool> IDataBaseConnectAsync()
    {
        return Task.FromResult(true);
    }
}
