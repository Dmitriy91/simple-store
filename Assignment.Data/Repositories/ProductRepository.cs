using Assignment.Entities;
using System.Data.Entity;

namespace Assignment.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>
    {
        public ProductRepository(DbContext _dbContext)
            : base(_dbContext)
        {
        }
    }
}
