using Store.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Store.Data.EntityConfigs
{
    public class OrderConfig : EntityTypeConfiguration<Order>
    {
        public OrderConfig()
        {
            HasMany(o => o.OrderDetails)
                .WithRequired()
                .HasForeignKey(od => od.OrderId);
        }
    }
}
