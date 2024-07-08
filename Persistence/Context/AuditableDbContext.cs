using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Persistence.Context
{
    public class AuditableDbContext : DbContext
    {
        public AuditableDbContext(DbContextOptions options) : base(options) { }

        public virtual async Task<int> SaveChangesAsync(int user = 0)
        {
            foreach (var entry in base.ChangeTracker.Entries<BaseAuditEntity>()
                .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.CreatedBy = user;
                }
                else
                {
                    entry.Entity.ModifiedOn = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = user;
                }
            }

            return await base.SaveChangesAsync();
        }

        public virtual async Task<int> SaveChangesNonUnitAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
