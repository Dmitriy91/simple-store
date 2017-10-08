using Store.Entities;
using System.Data.Entity;

namespace Store.Data.Repositories
{
    public class NaturalPersonRepository : RepositoryBase<NaturalPerson>
    {
        public NaturalPersonRepository(DbContext _dbContext)
            : base(_dbContext)
        {
        }
    }
}
