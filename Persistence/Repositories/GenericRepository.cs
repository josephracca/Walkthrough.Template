using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Domain.Common;

namespace Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditEntity
    {
        private readonly TemplateDbContext _dbContext;

        public GenericRepository(TemplateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            _dbContext.Entry(entity).State = EntityState.Added;
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
        }

        public async Task<T> Get(int id)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await _dbContext.Set<T>().Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task Update(T entity)
        {
            _dbContext.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task Delete(T entity)
        {
            entity.IsDeleted = true;
            _dbContext.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

        }
    }
}
