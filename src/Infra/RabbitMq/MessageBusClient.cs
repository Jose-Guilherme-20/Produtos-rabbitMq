using System.Text;
using System.Text.Json;
using Domain.Models.RabbitMq;
using RabbitMQ.Client;

namespace Infra.RabbitMq;

public class MessageBusClient : IMessageBusClient
{
    private readonly RabbitMqConnection _connection;
    private readonly RabbitMqSettings _settings;

    public MessageBusClient(
        RabbitMqConnection connection,
        Microsoft.Extensions.Options.IOptions<RabbitMqSettings> options)
    {
        _connection = connection;
        _settings = options.Value;
    }

    public async Task PublishAsync<T>(T message)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var properties = new BasicProperties
        {
            Persistent = true
        };

        await _connection.Channel!.BasicPublishAsync(
            exchange: "",
            routingKey: _settings.QueueName,
            mandatory: false,
            basicProperties: properties,
            body: body);
    }
}
