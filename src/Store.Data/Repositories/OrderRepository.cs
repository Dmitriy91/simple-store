using Store.Entities;
using System.Data.Entity;

namespace Store.Data.Repositories
{
    public class OrderRepository : RepositoryBase<Order>
    {
        public OrderRepository(DbContext _dbContext)
            : base(_dbContext)
        {
        }
    }
}
