using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Core.Entities;
using Core.Interfaces;
using Core.DTOs;


namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<Customer> _customerRepo;

        public OrderService(
            IGenericRepository<Order> orderRepo,
            IGenericRepository<Product> productRepo,
            IGenericRepository<Customer> customerRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _customerRepo = customerRepo;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var customer = await _customerRepo.GetByIdAsync(orderDto.CustomerId);
            if (customer == null) throw new Exception("Customer not found");

            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                ShippingAddress = orderDto.ShippingAddress,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach (var item in orderDto.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product {item.ProductId} not found");

                if (product.StockQuantity < item.Quantity)
                    throw new Exception($"Insufficient stock for product {product.Name}");

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                };

                totalAmount += orderItem.Price * orderItem.Quantity;
                order.OrderItems.Add(orderItem);

                product.StockQuantity -= item.Quantity;
                _productRepo.Update(product);
            }

            order.TotalAmount = totalAmount;

            _orderRepo.Add(order);
            await _orderRepo.SaveAsync();

            return order;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _orderRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var orders = await _orderRepo.ListAllAsync();
            return orders.Where(o => o.CustomerId == customerId);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            _orderRepo.Update(order);
            return await _orderRepo.SaveAsync();
        }
    }
}