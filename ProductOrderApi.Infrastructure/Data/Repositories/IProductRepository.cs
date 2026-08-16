using ProductOrderApi.Data.Entities;

namespace ProductOrderApi.Data.Repositories
{
    public interface IProductRepository
    {
        Task<Product> AddProduct(Product product);
        Task<bool> DeleteProduct(int id);
        Task<Product?> GetProductById(int id);
        Task<List<Product>> GetProductByIds(IEnumerable<int> ids);
        Task<List<Product>> GetProducts();
        Task<Product?> UpdateProduct(int id, Product updatedProduct);
    }
}