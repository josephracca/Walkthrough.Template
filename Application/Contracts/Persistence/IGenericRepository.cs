using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : BaseAuditEntity
    {
        Task<T> Get(int id);
        Task<IReadOnlyList<T>> GetAll();
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
