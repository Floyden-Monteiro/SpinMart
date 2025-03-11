using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> ProcessPaymentAsync(Payment payment);
        Task<Payment> GetPaymentByOrderIdAsync(int orderId);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, PaymentStatus status);
    }
}