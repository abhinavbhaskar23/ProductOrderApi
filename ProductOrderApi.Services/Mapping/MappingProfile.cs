using AutoMapper;
using ProductOrderApi.Data.Entities;
using ProductOrderApi.Dtos;

namespace ProductOrderApi.Mapping;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderProduct, OrderProductDto>()
    .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
    }
}
