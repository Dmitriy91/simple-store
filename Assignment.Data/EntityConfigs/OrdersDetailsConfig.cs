using Assignment.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Assignment.Data.EntityConfigs
{
    public class OrdersDetailsConfig : EntityTypeConfiguration<OrderDetails>
    {
        public OrdersDetailsConfig()
        {
            HasKey(od => new { od.OrderId, od.ProductId });
        }
    }
}
