
using Domain.Interfaces.Repository;
using Domain.Models.RabbitMq;
using Infra.RabbitMq;
using Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra
{
    public static class AddExtensionInfra
    {
        public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context.AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            services.Configure<RabbitMqSettings>(
                     configuration.GetSection("RabbitMQ"));

            services.AddSingleton<RabbitMqConnection>();
            services.AddHostedService<RabbitMqHostedService>();
            services.AddScoped<IMessageBusClient, MessageBusClient>();

            services.AddScoped<IProductRepository, ProductRepository>();

        }
    }
}
