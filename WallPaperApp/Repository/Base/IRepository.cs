using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WallPaperApp.Entity;
using WallPaperApp.Entity.Account;
using WallPaperApp.Entity.Base;

namespace WallPaperApp.Repository.Base
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        Task<List<T>> GetAll(Expression<Func<T, bool>> predicate);

        Task<List<T>> GetAllWithPage(int page, int pageSize = 10);

        Task<long> GetCount(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> predicate = null);


        Task<bool> UpdateDocument(string id, UpdateDefinition<T> update);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(string id);
        Task<T> AddAsync(T entity);
        Task<bool> AddRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(string id, T entity);
        Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate);
        Task<T> DeleteAsync(T entity);
        Task<T> DeleteAsync(string id);
        Task<T> DeleteAsync(Expression<Func<T, bool>> filter);
    }
}