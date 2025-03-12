using Core.Entities;

public class CreatePaymentDto
{
    public int OrderId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
}