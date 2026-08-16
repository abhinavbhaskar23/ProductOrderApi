using ProductOrderApi.Dtos;

namespace ProductOrderApi.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrder(CreateOrderDto model);
        Task<bool> DeleteOrder(int id);
        Task<OrderDto?> GetOrder(int id);
        Task<List<OrderDto>> GetOrders();
        Task<OrderDto?> UpdateOrder(int id, CreateOrderDto model);
    }
}