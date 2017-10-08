using Store.Entities;
using System.Data.Entity;

namespace Store.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>
    {
        public ProductRepository(DbContext _dbContext)
            : base(_dbContext)
        {
        }
    }
}
