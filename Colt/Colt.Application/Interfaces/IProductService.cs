using Colt.Domain.Entities;

namespace Colt.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
    }
}
