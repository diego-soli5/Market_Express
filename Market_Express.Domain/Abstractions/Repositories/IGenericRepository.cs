using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(Guid id, string includeProperties = null);
        IQueryable<TEntity> GetAll(string includeProperties = null);
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> where, string includeProperties = null);
        void Create(TEntity entity); 
        void Create(List<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task Delete(List<Guid> ids);
        void Delete(List<TEntity> entity);
        Task Delete(Guid id);
    }
}
