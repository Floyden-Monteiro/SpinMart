using Core.Entities;

namespace Core.Specification
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
            : base(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId) &&
                (x.StockQuantity >= productParams.MinStock)
            )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);

            if (productParams.Sort == "priceAsc")
                AddOrderBy(x => x.Price);
            else if (productParams.Sort == "priceDesc")
                AddOrderByDescending(x => x.Price);
            else if (productParams.Sort == "stockAsc")
                AddOrderBy(x => x.StockQuantity);

            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1),
                productParams.PageSize);
        }

        public ProductsWithTypesAndBrandsSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}