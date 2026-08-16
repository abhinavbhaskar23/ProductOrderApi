namespace ProductOrderApi.Models;

public class CreateOrderDto
{
    public List<CreateOrderProductDto> OrderProducts { get; set; } = new();
}
