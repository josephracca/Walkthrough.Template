using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using Domain;

namespace Persistence.Context
{
    public class TemplateDbContext : AuditableDbContext
    {
        public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        private DatabaseFacade Database { get; }
        public IDbConnection Connection => Database.GetDbConnection();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity => { entity.HasQueryFilter(u => !u.IsDeleted); });
        }
    }
}
