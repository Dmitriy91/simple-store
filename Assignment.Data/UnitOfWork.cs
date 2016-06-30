using System.Data.Entity;
using System.Threading.Tasks;

namespace Assignment.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Commit()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch
            {
                // Log errors here
                throw;
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                // Log errors here
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
