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

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id, CancellationToken.None);
            return await _productRepository.DeleteAsync(product, CancellationToken.None);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _productRepository.GetAsync(CancellationToken.None);
        }

        public async Task InsertAsync(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Description))
                throw new ArgumentException("Name and Description are required.");

            await _productRepository.AddAsync(product, CancellationToken.None);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(product.Id, CancellationToken.None);

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;

            return await _productRepository.UpdateAsync(existingProduct, CancellationToken.None);
        }
    }
}
