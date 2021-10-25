using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class GenericRepository<TEntity> : ADORepository, IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbSet<TEntity> _dbEntity;

        public GenericRepository(DbContext context, IConfiguration configuration)
            : base(configuration)
        {
            _dbEntity = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll(string sIncludeProperties = null)
        {
            IQueryable<TEntity> query = _dbEntity;

            if (sIncludeProperties != null)
            {
                foreach (var sIncludeProperty in sIncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(sIncludeProperty);
                }
            }

            return query.AsEnumerable();
        }

        public async Task<TEntity> GetByIdAsync(Guid id, string sIncludeProperties = null)
        {
            IQueryable<TEntity> query = _dbEntity;

            if (sIncludeProperties != null)
            {
                foreach (var sIncludeProperty in sIncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(sIncludeProperty);
                }
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> where,
                                         string sIncludeProperties = null)
        {
            IQueryable<TEntity> query = _dbEntity;

            if (where == null)
            {
                throw new ArgumentNullException(nameof(where));
            }

            query = query.Where(where);

            if (sIncludeProperties != null)
            {
                foreach (var sIncludeProperty in sIncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(sIncludeProperty);
                }
            }

            return query.FirstOrDefault();
        }

        public void Create(TEntity entity)
        {
            _dbEntity.Add(entity);
        }

        public void Create(List<TEntity> entities)
        {
            _dbEntity.AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            _dbEntity.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbEntity.Remove(entity);
        }

        public void Delete(List<TEntity> entity)
        {
            _dbEntity.RemoveRange(entity);
        }

        public async Task Delete(List<Guid> ids)
        {
            foreach (var id in ids)
            {
                Delete(await GetByIdAsync(id));
            }
        }

        public async Task Delete(Guid id)
        {
            Delete(await GetByIdAsync(id));
        }
    }
}
