using Domain.Interfaces.Repository;
using Infra.RabbitMq;

namespace Produtos_rabbitMq.UseCases.Product.ProductBuy
{
    public class ProductBuyUseCase : IProductBuyUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMessageBusClient _messageBusClient;

        public ProductBuyUseCase(IProductRepository productRepository, IMessageBusClient messageBusClient)
        {
            _productRepository = productRepository;
            _messageBusClient = messageBusClient;
        }

        public async Task ExecuteAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new Exception("Produto não encontrado.");
            }
            if (product.Stock <= 0)
            {
                throw new Exception("Produto sem estoque.");
            }
            product.Stock -= 1;
            await _productRepository.UpdateAsync(product);
            var message = new
            {
                ProductId = product.Id,
                Price = product.Price,
                Quantity = quantity,
                ProductName = product.Name,
                Timestamp = DateTime.UtcNow
            };
            await _messageBusClient.PublishAsync(message);
        }   
    }
}
