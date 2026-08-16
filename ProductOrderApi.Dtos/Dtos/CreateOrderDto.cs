namespace ProductOrderApi.Dtos;

public class CreateOrderDto
{
    public List<CreateOrderProductDto> OrderProducts { get; set; } = new();
}
