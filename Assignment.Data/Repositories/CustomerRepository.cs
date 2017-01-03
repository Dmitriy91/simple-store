using Assignment.Entities;
using System.Data.Entity;

namespace Assignment.Data.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>
    {
        public CustomerRepository(DbContext _dbContext)
            : base(_dbContext)
        {
        }
    }
}
