
using Domain.Models.Entities;

namespace Domain.Interfaces.Repository
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int id);
        Task UpdateAsync(Product product);
    }
}
