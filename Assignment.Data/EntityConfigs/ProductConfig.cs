using Assignment.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Assignment.Data.EntityConfigs
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
                .HasForeignKey(od => od.ProductId).WillCascadeOnDelete(false);
        }
    }
}
