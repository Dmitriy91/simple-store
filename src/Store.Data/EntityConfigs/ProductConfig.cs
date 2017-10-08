using Store.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Store.Data.EntityConfigs
{
    public class ProductConfig : EntityTypeConfiguration<Product>
    {
        public ProductConfig()
        {
            Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(128);
            HasMany(o => o.OrderDetails)
                .WithRequired()
                .HasForeignKey(od => od.ProductId)
                .WillCascadeOnDelete(false);
        }
    }
}
