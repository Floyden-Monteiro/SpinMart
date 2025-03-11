using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ProductBrandConfiguration : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            builder.Property(b => b.Id)
                .HasIdentityOptions(startValue: 1)
                .UseSerialColumn(); 

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(b => b.Name)
                .IsUnique();
        }
    }
}