using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGenericRepository<Payment> _paymentRepo;
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IGenericRepository<Payment> paymentRepo,
            IGenericRepository<Order> orderRepo,
            ILogger<PaymentService> logger)
        {
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
            _logger = logger;
        }

        public async Task<Payment> ProcessPaymentAsync(Payment payment)
        {
            try
            {
                var order = await _orderRepo.GetByIdAsync(payment.OrderId);
                if (order == null) return null;

                payment.Status = PaymentStatus.Pending;
                payment.PaymentDate = DateTime.UtcNow;

                _paymentRepo.Add(payment);
                await _paymentRepo.SaveAsync();

                // Update order status
                order.Status = OrderStatus.Processing;
                _orderRepo.Update(order);
                await _orderRepo.SaveAsync();

                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return null;
            }
        }

        public async Task<Payment> GetPaymentByOrderIdAsync(int orderId)
        {
            var payments = await _paymentRepo.ListAllAsync();
            return payments.FirstOrDefault(x => x.OrderId == orderId);
        }

        public async Task<bool> UpdatePaymentStatusAsync(int paymentId, PaymentStatus status)
        {
            try
            {
                var payment = await _paymentRepo.GetByIdAsync(paymentId);
                if (payment == null) return false;

                payment.Status = status;
                _paymentRepo.Update(payment);
                return await _paymentRepo.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment status");
                return false;
            }
        }
    }
}