using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Save(int userId = 0);
        Task RollbackTransaction();
        Task<IDbContextTransaction> BeginTransaction();
    }
}
