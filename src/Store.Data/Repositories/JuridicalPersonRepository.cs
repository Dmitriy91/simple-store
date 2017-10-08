using Store.Entities;
using System.Data.Entity;

namespace Store.Data.Repositories
{
    public class JuridicalPersonRepository : RepositoryBase<JuridicalPerson>
    {
        public JuridicalPersonRepository(DbContext _dbContext)
            : base(_dbContext)
        {
        }
    }
}
