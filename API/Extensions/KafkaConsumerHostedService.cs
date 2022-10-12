using KafkaFlow;
using System.Diagnostics;
using System.Text;

namespace API.Extensions
{
    internal class KafkaConsumerHostedService:BackgroundService
    {
        private readonly IServiceProvider _services;
        private IKafkaBus? _kafkaBus;

        public KafkaConsumerHostedService(
            IServiceProvider services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {

                _kafkaBus = _services.CreateKafkaBus();

                await _kafkaBus.StartAsync(stoppingToken);

                var sb = new StringBuilder("Consumers:");
                foreach (var consumer in _kafkaBus.Consumers.All)
                {
                    sb.Append($"   [{consumer.Status}] ClusterName: {consumer.ClusterName}, ClientInstanceName: {consumer.ClientInstanceName}, GroupId: {consumer.GroupId}, Topics: [{string.Join(", ", consumer.Topics)}]");
                }

            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Cancelled");
            }
            catch (Exception e)
            {
                Debug.WriteLine( "Error has occurred while starting kafka consumer: {msg}", e.Message);
                throw;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_kafkaBus != null)
            {
                try
                {
                    await _kafkaBus.StopAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error has occurred while stopping kafka consumer: {msg}", e.Message);
                }
            }

            await base.StopAsync(cancellationToken);
        }
    }
}