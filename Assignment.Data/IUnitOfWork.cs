using System;
using System.Threading.Tasks;

namespace Assignment.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        Task CommitAsync();
    }
}
