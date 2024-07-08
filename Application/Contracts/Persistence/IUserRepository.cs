using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Contracts.Persistence
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmail(string email);
        Task<bool> IsValidIdentifier(Guid guid);
        Task<User> GetByIdentifier(Guid guid);
    }
}
