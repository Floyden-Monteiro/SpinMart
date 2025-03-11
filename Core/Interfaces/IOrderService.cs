using Core.Entities;
using Core.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderDto orderDto);
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
}