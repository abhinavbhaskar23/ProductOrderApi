
using ProductOrderApi.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderProductDto> OrderProducts { get; set; } = new();
}
