using ProductOrderApi.Data.Entities;

namespace ProductOrderApi.Data.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddOrder(Order order);
        Task<bool> DeleteOrder(int id);
        Task<Order?> GetOrder(int id);
        Task<List<Order>> GetOrders();
        Task<Order?> UpdateOrder(int id, Order updatedOrder);
    }
}