using ProductOrderApi.Models;

namespace ProductOrderApi.Services
{
    public interface IProductService
    {
        Task<ProductDto> AddProduct(CreateProductDto createProductDto);
        Task<bool> DeleteProduct(int id);
        Task<ProductDto?> GetProductById(int id);
        Task<List<ProductDto>> GetProducts();
        Task<ProductDto?> UpdateProduct(int id, UpdateProductDto productDto);
    }
}