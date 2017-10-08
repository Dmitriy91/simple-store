using Store.Entities;
using System.Data.Entity;

namespace Store.Data.Repositories
{
    public class OrderDetailsRepository : RepositoryBase<OrderDetails>
    {
        public OrderDetailsRepository(DbContext _dbContext)
            : base(_dbContext)
        {
        }
    }
}
