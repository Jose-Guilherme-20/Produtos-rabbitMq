
using Domain.Models.RabbitMq;
using Microsoft.Extensions.Options;
using Polly;
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
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    6,
                    retry => TimeSpan.FromSeconds(Math.Pow(2, retry)),
                    (ex, time, retryCount, ctx) =>
                    {
                        Console.WriteLine($"RabbitMQ não disponível. Tentativa {retryCount}. Nova tentativa em {time.TotalSeconds}s");
                    });

            await retryPolicy.ExecuteAsync(async () =>
            {
                Connection = await factory.CreateConnectionAsync();
                Channel = await Connection.CreateChannelAsync();

                await Channel.QueueDeclareAsync(
                    queue: _settings.QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            });
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
