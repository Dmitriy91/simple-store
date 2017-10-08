using System;
using System.Threading.Tasks;

namespace Store.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        Task CommitAsync();
    }
}
