using KafkaFlow;
using System.Diagnostics;

namespace API.Services
{
    internal class KafkaConsumerHostedService : BackgroundService
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
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Cancelled");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error has occurred while starting kafka consumer: {msg}", e.Message);
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