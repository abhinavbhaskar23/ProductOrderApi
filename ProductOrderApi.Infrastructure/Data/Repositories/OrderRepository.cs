using Microsoft.EntityFrameworkCore;
using ProductOrderApi.Data.Entities;

namespace ProductOrderApi.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;
        public OrderRepository(OrderContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _context.Orders.Include(o => o.OrderProducts).ThenInclude(op => op.Product).ToListAsync();
        }

        public async Task<Order?> GetOrder(int id)
        {
            return await _context.Orders.Include(o => o.OrderProducts).ThenInclude(op => op.Product).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> AddOrder(Order order)
        {
            _context.Orders.Add(order);
            return order;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }
            _context.Orders.Remove(order);
            return true;
        }

        public async Task<Order?> UpdateOrder(int id, Order updatedOrder)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return null;
            }

            order.OrderDate = updatedOrder.OrderDate;

            // Remove line items that are no longer present
            var updatedProductIds = updatedOrder.OrderProducts?.Select(op => op.ProductId).ToList() ?? new List<int>();
            var itemsToRemove = order.OrderProducts?
                .Where(op => !updatedProductIds.Contains(op.ProductId))
                .ToList() ?? new List<OrderProduct>();

            foreach (var item in itemsToRemove)
            {
                order.OrderProducts!.Remove(item);
            }

            decimal totalPrice = 0;

            // Update existing items or add new ones
            foreach (var updatedItem in updatedOrder.OrderProducts ?? new List<OrderProduct>())
            {
                var existingItem = order.OrderProducts?
                    .FirstOrDefault(op => op.ProductId == updatedItem.ProductId);

                if (existingItem != null)
                {
                    existingItem.Quantity = updatedItem.Quantity;
                    existingItem.Price = updatedItem.Price;
                }
                else
                {
                    order.OrderProducts!.Add(new OrderProduct
                    {
                        ProductId = updatedItem.ProductId,
                        Quantity = updatedItem.Quantity,
                        Price = updatedItem.Price
                    });
                }

                totalPrice += updatedItem.Price * updatedItem.Quantity;
            }

            order.TotalPrice = totalPrice;

            return order;
        }
    }
}
