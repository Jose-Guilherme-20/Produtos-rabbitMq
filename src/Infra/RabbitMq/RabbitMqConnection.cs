
using Domain.Models.RabbitMq;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infra.RabbitMq
{
    public class RabbitMqConnection : IAsyncDisposable
    {
        private readonly RabbitMqSettings _settings;

        public IConnection? Connection { get; private set; }
        public IChannel? Channel { get; private set; }

        public RabbitMqConnection(IOptions<RabbitMqSettings> options)
        {
            _settings = options.Value;
        }

        public async Task InitializeAsync()
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_settings.Host),
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            Connection = await factory.CreateConnectionAsync();
            Channel = await Connection.CreateChannelAsync();

            await Channel.QueueDeclareAsync(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public async ValueTask DisposeAsync()
        {
            if (Channel != null)
                await Channel.DisposeAsync();

            if (Connection != null)
                await Connection.DisposeAsync();
        }
    }
}
