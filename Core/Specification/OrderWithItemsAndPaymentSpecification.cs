using Core.Entities;

namespace Core.Specification
{
    public class OrderWithItemsAndPaymentSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsAndPaymentSpecification()
        {
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.OrderItems.Select(oi => oi.Product));
            AddInclude(o => o.Payment);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrderWithItemsAndPaymentSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.OrderItems.Select(oi => oi.Product));
            AddInclude(o => o.Payment);
        }
    }
}