using Core.Entities;
namespace Core.Specification;

public class ProductWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductWithTypesAndBrandsSpecification()
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }

    public ProductWithTypesAndBrandsSpecification(int id)
        : base(x => x.Id == id)
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }

   
    public ProductWithTypesAndBrandsSpecification(int threshold, bool isLowStock = false)
        : base(x => isLowStock ? x.StockQuantity <= threshold : true)
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
        AddOrderBy(x => x.Name);  
    }
}