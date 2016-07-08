using Assignment.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Assignment.Data.EntityConfigs
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
