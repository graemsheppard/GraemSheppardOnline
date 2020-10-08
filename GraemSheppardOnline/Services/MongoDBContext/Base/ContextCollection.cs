using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace GraemSheppardOnline.Services.MongoDBContext
{
    public class ContextCollection<T>
    {
        protected IMongoCollection<T> Collection;

        public ContextCollection(IMongoCollection<T> col)
        {
            Collection = col;
        }

        #region CREATE

        public void InsertOne(T document, InsertOneOptions options = null)
        {
            Collection.InsertOne(document, options);
        }

        public async Task InsertOneAsync(T document,
                                          InsertOneOptions options = null,
                                          CancellationToken cancellationToken = default(CancellationToken))
        {
            await Collection.InsertOneAsync(document, options, cancellationToken);
        }

        public void InsertMany(IEnumerable<T> documents, InsertManyOptions options = null)
        {
            Collection.InsertMany(documents, options);
        }

        public async Task InsertManyAsync(IEnumerable<T> documents,
                                           InsertManyOptions options = default(InsertManyOptions),
                                           CancellationToken cancellationToken = default(CancellationToken))
        {
            await Collection.InsertManyAsync(documents, options, cancellationToken);

        }

        #endregion CREATE

        #region READ

        public IQueryable<T> AsQueryable()
        {
            return Collection.AsQueryable();
        }

        public IFindFluent<T, T> Find(FilterDefinition<T> filter, FindOptions options = null)
        {
            return Collection.Find(filter, options);
        }

        public IFindFluent<T, T> Find(Expression<Func<T, bool>> filter, FindOptions options = null)
        {
            return Collection.Find(filter, options);
        }

        public T FindOneAndUpdate(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, FindOneAndUpdateOptions<T> options = null)
        {
            return Collection.FindOneAndUpdate<T>(filter, update, options);
        }

        #endregion READ

        #region UPDATE

        public void UpdateOne(FilterDefinition<T> filter,
                               UpdateDefinition<T> update,
                               UpdateOptions options = null)
        {
            Collection.UpdateOne(filter, update, options);
        }

        public void UpdateOne(Expression<Func<T, bool>> filter,
                               UpdateDefinition<T> update,
                               UpdateOptions options = null)
        {
            Collection.UpdateOne(filter, update, options);
        }

        public async Task UpdateOneAsync(Expression<Func<T, bool>> filter,
                                           UpdateDefinition<T> update,
                                           UpdateOptions options = null,
                                           CancellationToken cancellationToken = default(CancellationToken))
        {
            await Collection.UpdateOneAsync(filter, update, options, cancellationToken);
        }

        public async Task UpdateOneAsync(FilterDefinition<T> filter,
                                           UpdateDefinition<T> update,
                                           UpdateOptions options = null,
                                           CancellationToken cancellationToken = default(CancellationToken))
        {
            await Collection.UpdateOneAsync(filter, update, options, cancellationToken);
        }

        

        public void UpdateMany(FilterDefinition<T> filter,
                                UpdateDefinition<T> update,
                                UpdateOptions options = null)
        {
            Collection.UpdateManyAsync(filter, update, options);
        }

        public void UpdateMany(Expression<Func<T, bool>> filter,
                                UpdateDefinition<T> update,
                                UpdateOptions options = null)
        {
            Collection.UpdateManyAsync(filter, update, options);
        }

        public async Task UpdateManyAsync(FilterDefinition<T> filter,
                                     UpdateDefinition<T> update,
                                     UpdateOptions options = null,
                                     CancellationToken cancellationToken = default(CancellationToken))
        {
            await Collection.UpdateManyAsync(filter, update, options, cancellationToken);
        }

        public async Task UpdateManyAsync(Expression<Func<T, bool>> filter,
                                     UpdateDefinition<T> update,
                                     UpdateOptions options = null,
                                     CancellationToken cancellationToken = default(CancellationToken))
        {
            await Collection.UpdateManyAsync(filter, update, options, cancellationToken);
        }

        #endregion UPDATE

        #region DELETE

        public void DeleteOne(FilterDefinition<T> filter, DeleteOptions options = null)
        {
            Collection.DeleteOne(filter, options);
        }

        public async Task DeleteOneAsync(FilterDefinition<T> filter,
                                          DeleteOptions options = null,
                                          CancellationToken cancellationToken = default(CancellationToken))
        {
            await Collection.DeleteOneAsync(filter, options, cancellationToken);
        }

        public async Task DeleteOneAsync(Expression<Func<T, bool>> filter,
                                          DeleteOptions options = null,
                                          CancellationToken cancellationToken = default(CancellationToken))
        {
            await Collection.DeleteOneAsync(filter, options, cancellationToken);
        }

        public void DeleteMany(FilterDefinition<T> filter,
                                DeleteOptions options = null)
        {
            Collection.DeleteMany(filter, options);
        }

        public void DeleteMany(Expression<Func<T, bool>> filter,
                                DeleteOptions options = null)
        {
            Collection.DeleteMany(filter, options);
        }

        public async Task DeleteManyAsync(FilterDefinition<T> filter,
                                           DeleteOptions options = null,
                                           CancellationToken cancellationToken = default(CancellationToken))
        {
            await Collection.DeleteManyAsync(filter, options, cancellationToken);
        }

        public async Task DeleteManyAsync(Expression<Func<T, bool>> filter,
                                           DeleteOptions options = null,
                                           CancellationToken cancellationToken = default(CancellationToken))
        {
            await Collection.DeleteManyAsync(filter, options, cancellationToken);
        }

        #endregion DELETE

    }
}
