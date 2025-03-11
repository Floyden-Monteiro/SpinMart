using Core.Entities;

namespace Core.Specification
{
    public class BrandSpecification : BaseSpecification<ProductBrand>
    {
        public BrandSpecification(int pageIndex, int pageSize)
            : base()
        {
            AddOrderBy(x => x.Name);
            ApplyPaging((pageIndex - 1) * pageSize, pageSize);
        }

        public BrandSpecification(int id)
            : base(x => x.Id == id)
        {
        }
    }

    public class BrandWithFiltersForCountSpecification : BaseSpecification<ProductBrand>
    {
        public BrandWithFiltersForCountSpecification() : base()
        {
        }
    }
}