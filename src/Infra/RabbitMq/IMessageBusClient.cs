
namespace Infra.RabbitMq
{
    public interface IMessageBusClient
    {
        Task PublishAsync<T>(T message);
    }
}
