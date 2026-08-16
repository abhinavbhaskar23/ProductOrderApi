using Microsoft.AspNetCore.Mvc;
using ProductOrderApi.Models;
using ProductOrderApi.Services;

namespace ProductOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            var orders = await _orderService.GetOrders();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderService.GetOrder(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
        {
            var created = await _orderService.CreateOrder(createOrderDto);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, CreateOrderDto updateOrderDto)
        {
            var updated = await _orderService.UpdateOrder(id, updateOrderDto);
            if (updated == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleted = await _orderService.DeleteOrder(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}