
using Microsoft.Extensions.Hosting;

namespace Infra.RabbitMq
{
    public class RabbitMqHostedService : IHostedService
    {
        private readonly RabbitMqConnection _rabbitConnection;

        public RabbitMqHostedService(RabbitMqConnection rabbitConnection)
        {
            _rabbitConnection = rabbitConnection;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _rabbitConnection.InitializeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
