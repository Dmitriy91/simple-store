using Store.Entities;
using System.Data.Entity;

namespace Store.Data.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>
    {
        public CustomerRepository(DbContext _dbContext)
            : base(_dbContext)
        {
        }
    }
}
