using Core.Entities;

namespace Core.Specification
{
    public class CustomerWithOrdersSpecification : BaseSpecification<Customer>
    {
        public CustomerWithOrdersSpecification(CustomerSpecParams customerParams)
            : base(x =>
                (string.IsNullOrEmpty(customerParams.Search) ||
                x.Name.ToLower().Contains(customerParams.Search) ||
                x.Email.ToLower().Contains(customerParams.Search))
            )
        {
            AddInclude(x => x.Orders);

            if (customerParams.SortBy == "nameAsc")
                AddOrderBy(x => x.Name);
            else if (customerParams.SortBy == "nameDesc")
                AddOrderByDescending(x => x.Name);
            else if (customerParams.SortBy == "recent")
                AddOrderByDescending(x => x.CreatedAt);

            ApplyPaging(customerParams.PageSize * (customerParams.PageIndex - 1),
                customerParams.PageSize);
        }

        public CustomerWithOrdersSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Orders);
        }
    }
}