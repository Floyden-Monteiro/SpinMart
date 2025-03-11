using Core.Entities;
using Core.Interfaces;
using Core.Specification;

namespace Infrastructure.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IGenericRepository<Product> _productRepo;

        public InventoryService(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<bool> UpdateStockQuantityAsync(int productId, int quantity)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null) return false;

            product.StockQuantity = quantity;
            _productRepo.Update(product);
            return await _productRepo.SaveAsync();
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 5)
        {
            var products = await _productRepo.ListAllAsync();
            return products.Where(p => p.StockQuantity <= threshold);
        }

        public async Task<bool> IsInStockAsync(int productId, int quantity)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            return product != null && product.StockQuantity >= quantity;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _productRepo.Add(product);
            await _productRepo.SaveAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null) return false;

            _productRepo.Delete(product);
            return await _productRepo.SaveAsync();
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _productRepo.Update(product);
            await _productRepo.SaveAsync();
            return product;
        }
    }
}