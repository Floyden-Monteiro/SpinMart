namespace API.DTOs
{
    public class PaymentSummaryDto
    {
        public int TotalPayments { get; set; }
        public decimal TotalAmount { get; set; }
        public int CompletedPayments { get; set; }
        public int PendingPayments { get; set; }
    }
}