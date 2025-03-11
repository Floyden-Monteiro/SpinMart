namespace Core.Entities
{
    public enum PaymentMethod
    {
        Cash,
        CreditCard,
        PayPal,
        Stripe
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }
}