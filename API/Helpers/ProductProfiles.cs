using API.Dtos;
using API.DTOs;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class ProductProfiles : Profile
    {
        public ProductProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name));

            CreateMap<Customer, CustomerToReturnDto>()
                .ForMember(d => d.OrderCount, o => o.MapFrom(s => s.Orders.Count));
            CreateMap<CustomerCreateDto, Customer>();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
                .ForMember(d => d.Total, o => o.MapFrom(s => s.Price * s.Quantity));

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.Name))
                .ForMember(d => d.OrderStatus, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.OrderItems));

            CreateMap<OrderItem, OrderItemToReturnDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
                .ForMember(d => d.Total, o => o.MapFrom(s => s.Price * s.Quantity));

            CreateMap<CreateOrderDto, Order>();
            CreateMap<CreateOrderItemDto, OrderItem>();

            // Simple brand mappings
            CreateMap<ProductBrand, BrandToReturnDto>();
            CreateMap<BrandCreateDto, ProductBrand>();
            CreateMap<BrandUpdateDto, ProductBrand>();
            CreateMap<OrderItem, OrderItemToReturnDto>()
          .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
          .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
          .ForMember(d => d.Total, o => o.MapFrom(s => s.Price * s.Quantity));

            CreateMap<Payment, PaymentToReturnDto>()
                .ForMember(d => d.PaymentMethod, o => o.MapFrom(s => s.PaymentMethod.ToString()))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        }
    }
}
