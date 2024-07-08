using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Models;
using Domain;

namespace Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly TemplateDbContext _dbContext;

        public UserRepository(TemplateDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _dbContext.Users
                .Where(u => u.EmailAddress == email)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByIdentifier(Guid guid)
        {
            return await _dbContext.Users
                .Where(u => u.Identifier == guid)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsValidIdentifier(Guid guid)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Identifier == guid && u.IdentifierExpirationDate < DateTime.UtcNow);
        }
    }
}
