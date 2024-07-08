using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Persistence.Context;
using Application.Constant;
using Application.Contracts.Persistence;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TemplateDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private string _systemUser;
        public UnitOfWork(
            TemplateDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _context = context;
            this._httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _systemUser = _configuration.GetValue<string>("SystemUser");
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> Save(int user = 0)
        {
            string userId;
            if (user == 0)
            {
                userId = _httpContextAccessor.HttpContext.User.FindFirst(UserClaim.Id)?.Value;
                if (userId == null)
                {
                    userId = _systemUser;
                };
            }
            else
            {
                userId = user.ToString();
            }

            return await _context.SaveChangesAsync(Int32.Parse(userId));
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task RollbackTransaction()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
