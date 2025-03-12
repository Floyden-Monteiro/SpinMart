using System;
using System.Collections.Generic;
using Core.Extensions;

namespace Core.Entities
{
    public class Order : BaseEntity
    {
        private DateTime _orderDate;

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime OrderDate
        {
            get => _orderDate;
            set => _orderDate = value.ToUtc();
        }

        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public Payment Payment { get; set; }
    }
}