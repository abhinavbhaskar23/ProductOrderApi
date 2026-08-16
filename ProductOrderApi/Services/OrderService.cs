using AutoMapper;
using ProductOrderApi.Data;
using ProductOrderApi.Data.Entities;
using ProductOrderApi.Models;

namespace ProductOrderApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> GetOrders()
        {
            var orders = await _unitOfWork.Orders.GetOrders();
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto?> GetOrder(int id)
        {
            var order = await _unitOfWork.Orders.GetOrder(id);
            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateOrder(CreateOrderDto model)
        {
            var productIds = model.OrderProducts.Select(op => op.ProductId).ToList();
            var products = await _unitOfWork.Products.GetProductByIds(productIds);

            var orderProducts = new List<OrderProduct>();
            decimal totalPrice = 0;

            foreach (var item in model.OrderProducts)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product with id {item.ProductId} does not exist");
                }

                orderProducts.Add(new OrderProduct
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    Price = product.Price
                    // Product navigation intentionally left unset here —
                    // see note below on why we re-fetch instead of attaching it directly.
                });

                totalPrice += product.Price * item.Quantity;
            }

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                OrderProducts = orderProducts,
                TotalPrice = totalPrice
            };

            var savedOrder = await _unitOfWork.Orders.AddOrder(order);
            await _unitOfWork.SaveChangesAsync();

            // Re-fetch with Include(...).ThenInclude(...) so OrderProducts[].Product
            // is populated correctly for mapping ProductName in OrderDto.
            var fullOrder = await _unitOfWork.Orders.GetOrder(savedOrder.Id);
            return _mapper.Map<OrderDto>(fullOrder);
        }

        public async Task<OrderDto?> UpdateOrder(int id, CreateOrderDto model)
        {
            var productIds = model.OrderProducts.Select(op => op.ProductId).ToList();
            var products = await _unitOfWork.Products.GetProductByIds(productIds);

            var orderProducts = new List<OrderProduct>();
            decimal totalPrice = 0;

            foreach (var item in model.OrderProducts)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product with id {item.ProductId} does not exist");
                }

                orderProducts.Add(new OrderProduct
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    Price = product.Price
                });

                totalPrice += product.Price * item.Quantity;
            }

            var updatedOrder = new Order
            {
                OrderDate = DateTime.UtcNow,
                OrderProducts = orderProducts,
                TotalPrice = totalPrice
            };

            var result = await _unitOfWork.Orders.UpdateOrder(id, updatedOrder);
            if (result == null) return null;
            await _unitOfWork.SaveChangesAsync();

            var fullOrder = await _unitOfWork.Orders.GetOrder(id);
            return _mapper.Map<OrderDto>(fullOrder);
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var deleted = await _unitOfWork.Orders.DeleteOrder(id);
            if (!deleted)
                return false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}