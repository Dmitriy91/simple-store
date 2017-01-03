using Assignment.Entities;
using System.Data.Entity;

namespace Assignment.Data.Repositories
{
    public class OrderDetailsRepository : RepositoryBase<OrderDetails>
    {
        public OrderDetailsRepository(DbContext _dbContext)
            : base(_dbContext)
        {
        }
    }
}
