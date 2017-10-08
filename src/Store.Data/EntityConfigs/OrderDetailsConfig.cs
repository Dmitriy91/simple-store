using Store.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Store.Data.EntityConfigs
{
    public class OrderDetailsConfig : EntityTypeConfiguration<OrderDetails>
    {
        public OrderDetailsConfig()
        {
            HasKey(od => new { od.OrderId, od.ProductId });
            HasRequired(od => od.Order)
                .WithMany(o => o.OrderDetails);
        }
    }
}
