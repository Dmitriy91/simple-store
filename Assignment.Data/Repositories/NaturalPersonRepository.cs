using Assignment.Entities;
using System.Data.Entity;

namespace Assignment.Data.Repositories
{
    public class NaturalPersonRepository : RepositoryBase<NaturalPerson>
    {
        public NaturalPersonRepository(DbContext _dbContext)
            : base(_dbContext)
        {
        }
    }
}
