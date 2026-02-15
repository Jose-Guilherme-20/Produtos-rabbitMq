using Produtos_rabbitMq.UseCases.Product.ProductBuy;

namespace Produtos_rabbitMq.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProductBuyUseCase, ProductBuyUseCase>();
        }
    }
}
