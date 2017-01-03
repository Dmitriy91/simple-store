using Assignment.Entities;
using System.Data.Entity;

namespace Assignment.Data.Repositories
{
    public class OrderRepository : RepositoryBase<Order>
    {
        public OrderRepository(DbContext _dbContext)
            : base(_dbContext)
        {
        }
    }
}
