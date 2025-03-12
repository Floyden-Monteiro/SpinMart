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
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => GetPaymentMethodName(src.PaymentMethod)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetPaymentStatusName(src.Status)))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency));

            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<Payment, PaymentToReturnDto>()
                .ForMember(dest => dest.PaymentMethod,
                    opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
        }

        private string GetPaymentMethodName(PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.Cash => "Cash",
                PaymentMethod.CreditCard => "Credit Card",
                PaymentMethod.PayPal => "PayPal",
                PaymentMethod.Stripe => "Stripe",
                _ => "Unknown"
            };
        }

        private string GetPaymentStatusName(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "Pending",
                PaymentStatus.Completed => "Completed",
                PaymentStatus.Failed => "Failed",
                PaymentStatus.Refunded => "Refunded",
                _ => "Unknown"
            };
        }
    }
}