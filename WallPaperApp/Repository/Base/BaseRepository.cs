using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using WallPaperApp.Entity;
using WallPaperApp.Entity.Account;
using WallPaperApp.Entity.Base;
using WallPaperApp.Infrastructure;
using WallPaperApp.Utility.Extensions;


namespace WallPaperApp.Repository.Base
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly IMongoCollection<T> _collection;

        // private IMongoCollection<T> collection => _collection;

        protected readonly IMongoDatabase Db;

        protected BaseRepository(IOptions<MongoDbSettings> options)
        {
            var settings = options.Value;
            var client = new MongoClient(settings.ConnectionString);
            Db = client.GetDatabase(settings.Database);
            _collection = Db.GetCollectionExt<T>();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public virtual IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? _collection.AsQueryable()
                : _collection.AsQueryable().Where(predicate);
        }


        public virtual async Task<List<T>> GetAllWithPage(int page, int pageSize = 10)
        {
            var filter = Builders<T>.Filter.Empty;

            var data = await _collection.Find(filter)
                .SortByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return data;
        }


        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync();
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<long> GetCount(Expression<Func<T, bool>> predicate)
        {
            return  await _collection.Find(predicate).CountDocumentsAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var options = new InsertOneOptions {BypassDocumentValidation = false};
            await _collection.InsertOneAsync(entity, options);
            return entity;
        }

        public virtual async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions {IsOrdered = false, BypassDocumentValidation = false};
            return (await _collection.BulkWriteAsync((IEnumerable<WriteModel<T>>) entities, options)).IsAcknowledged;
        }

        public virtual async Task<T> UpdateAsync(string id, T entity)
        {
            return await _collection.FindOneAndReplaceAsync(x => x.Id == id, entity);
        }

        public virtual async Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate)
        {
            return await _collection.FindOneAndReplaceAsync(predicate, entity);
        }

        public virtual async Task<T> DeleteAsync(T entity)
        {
            return await _collection.FindOneAndDeleteAsync(x => x.Id == entity.Id);
        }

        public virtual async Task<T> DeleteAsync(string id)
        {
            return await _collection.FindOneAndDeleteAsync(x => x.Id == id);
        }

        public virtual async Task<T> DeleteAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection.FindOneAndDeleteAsync(filter);
        }

        public virtual async Task<bool> UpdateDocument(string id, UpdateDefinition<T> update)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, id);
            await _collection.UpdateOneAsync(filter, update);
            return true;
        }
    }
}