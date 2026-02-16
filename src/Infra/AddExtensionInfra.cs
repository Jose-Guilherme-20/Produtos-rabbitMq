
using Domain.Interfaces.Repository;
using Domain.Models.RabbitMq;
using Infra.RabbitMq;
using Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infra
{
    public static class AddExtensionInfra
    {
        public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<Context.AppDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));

            services.Configure<RabbitMqSettings>(
                     configuration.GetSection("RabbitMQ"));

            services.AddSingleton<RabbitMqConnection>();
            services.AddHostedService<RabbitMqHostedService>();
            services.AddScoped<IMessageBusClient, MessageBusClient>();

            services.AddScoped<IProductRepository, ProductRepository>();

        }
    }
}
