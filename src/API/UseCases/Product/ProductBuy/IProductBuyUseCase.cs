namespace Produtos_rabbitMq.UseCases.Product.ProductBuy
{
    public interface IProductBuyUseCase
    {
        Task ExecuteAsync(int productId, int quantity);
    }
}
