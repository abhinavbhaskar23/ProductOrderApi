using AutoMapper;
using ProductOrderApi.Data;
using ProductOrderApi.Data.Entities;
using ProductOrderApi.Models;

namespace ProductOrderApi.Services
{

    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var products = await _unitOfWork.Products.GetProducts();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetProductById(int id)
        {
            var product = await _unitOfWork.Products.GetProductById(id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> AddProduct(CreateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);
            product.CreatedAt = DateTime.UtcNow;
            var addedProduct = await _unitOfWork.Products.AddProduct(product);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductDto>(addedProduct);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var deleted = await _unitOfWork.Products.DeleteProduct(id);
            if (!deleted)
                return false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<ProductDto?> UpdateProduct(int id, UpdateProductDto productDto)
        {
            var updatedProduct = _mapper.Map<Product>(productDto);
            updatedProduct.LastUpdatedAt = DateTime.UtcNow;
            var product = await _unitOfWork.Products.UpdateProduct(id, updatedProduct);
            if (product == null)
                return null;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

    }
}
