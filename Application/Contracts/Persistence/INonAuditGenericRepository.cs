using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Application.Contracts.Persistence
{
    public interface INonAuditGenericRepository<T> where T : NonAuditBaseEntity
    {
        Task<T> Get(int id);
        Task<T> Get(long id);
        Task<IReadOnlyList<T>> GetAll();
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
    }
}
