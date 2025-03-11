using Core.Entities;

namespace Core.Interfaces
{
    public interface IInventoryService
    {
        Task<bool> UpdateStockQuantityAsync(int productId, int quantity);
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 5);
        Task<bool> IsInStockAsync(int productId, int quantity);
        Task<Product> CreateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
        Task<Product> UpdateProductAsync(Product product);
    }
}