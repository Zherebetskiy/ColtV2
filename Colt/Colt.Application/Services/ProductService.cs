using Colt.Application.Interfaces;
using Colt.Domain.Entities;
using Colt.Domain.Repositories;

namespace Colt.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAsync(CancellationToken.None);
        }

        public async Task AddProductAsync(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Description))
                throw new ArgumentException("Name and Description are required.");

            await _productRepository.AddAsync(product, CancellationToken.None);
        }
    }
}
