using System;
using System.Collections.Generic;
using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public List<OrderItemToReturnDto> Items { get; set; }
        public PaymentToReturnDto Payment { get; set; }
    }

    public class OrderItemToReturnDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }

    public class CreateOrderDto
    {
        [Required]
        public int CustomerId { get; set; }
        
        [Required]
        public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
        
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        
        [Required]
        [StringLength(180)]
        public string ShippingAddress { get; set; }
    }

    public class CreateOrderItemDto
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class PaymentToReturnDto
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}