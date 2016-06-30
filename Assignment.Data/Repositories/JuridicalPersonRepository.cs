using Assignment.Entities;
using System.Data.Entity;

namespace Assignment.Data.Repositories
{
    public class JuridicalPersonRepository : RepositoryBase<JuridicalPerson>
    {
        public JuridicalPersonRepository(DbContext _dbContext)
            : base(_dbContext)
        { }
    }
}
