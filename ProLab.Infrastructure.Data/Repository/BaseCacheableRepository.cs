using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using ProLab.Infrastructure.Core;
using ProLab.Infrastructure.Core.Caching;
using ProLab.Infrastructure.Core.Events;

namespace ProLab.Infrastructure.Data
{
    /// <summary>
    /// Represents the entity repository implementation
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public partial class BaseCacheableRepository<TEntity> : ICacheableRepository<TEntity> where TEntity : BaseEntity
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly DbContext _dataProvider;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        public BaseCacheableRepository(IEventPublisher eventPublisher,
            DbContext dataProvider,
            ICacheManager staticCacheManager)
        {
            _eventPublisher = eventPublisher;
            _dataProvider = dataProvider;
            _cacheManager = staticCacheManager;
        }

        #endregion

        #region Utilities
        /// <summary>
        /// Add entity data integrity
        /// </summary>
        /// <param name="entity">entity</param>
        protected virtual void AddDataIntegrity(TEntity entity)
        {
            if (typeof(TEntity).GetInterface(nameof(IDataIntegrityEntity)) == null)
                return;

            (entity as IDataIntegrityEntity).ApplyDataIntegrity();

        }
        /// <summary>
        /// Add entity data integrity
        /// </summary>
        /// <param name="entities">collection of entity</param>
        protected virtual void AddDataIntegrity(IList<TEntity> entities)
        {
            if (typeof(TEntity).GetInterface(nameof(IDataIntegrityEntity)) == null)
                return;

            foreach(var model in entities)
                (model as IDataIntegrityEntity).ApplyDataIntegrity();


        }
        

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="getAllAsync">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the entity entries
        /// </returns>
        protected virtual async Task<IList<TEntity>> GetEntitiesAsync(Func<Task<IList<TEntity>>> getAllAsync, Func<ICacheManager, CacheKey> getCacheKey)
        {
            if (getCacheKey == null)
                return await getAllAsync();

            //caching
            var cacheKey = getCacheKey(_cacheManager)
                           ?? _cacheManager.PrepareKeyForDefaultCache(CacheDefaults<TEntity>.AllCacheKey);
            return await _cacheManager.GetAsync(cacheKey, getAllAsync);
        }

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="getAll">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>Entity entries</returns>
        protected virtual IList<TEntity> GetEntities(Func<IList<TEntity>> getAll, Func<ICacheManager, CacheKey> getCacheKey)
        {
            if (getCacheKey == null)
                return getAll();

            //caching
            var cacheKey = getCacheKey(_cacheManager)
                           ?? _cacheManager.PrepareKeyForDefaultCache(CacheDefaults<TEntity>.AllCacheKey);

            return _cacheManager.GetAsync(cacheKey, getAll).Result;
        }

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="getAllAsync">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the entity entries
        /// </returns>
        protected virtual async Task<IList<TEntity>> GetEntitiesAsync(Func<Task<IList<TEntity>>> getAllAsync, Func<ICacheManager, Task<CacheKey>> getCacheKey)
        {
            if (getCacheKey == null)
                return await getAllAsync();

            //caching
            var cacheKey = await getCacheKey(_cacheManager)
                           ?? _cacheManager.PrepareKeyForDefaultCache(CacheDefaults<TEntity>.AllCacheKey);
            return await _cacheManager.GetAsync(cacheKey, getAllAsync);
        }

        /// <summary>
        /// Adds "deleted" filter to query which contains <see cref="ISoftDeletedEntity"/> entries, if its need
        /// </summary>
        /// <param name="query">Entity entries</param>
        /// <param name="includeDeleted">Whether to include deleted items</param>
        /// <returns>Entity entries</returns>
        protected virtual IQueryable<TEntity> AddDeletedFilter(IQueryable<TEntity> query, in bool includeDeleted)
        {
            if (includeDeleted)
                return query;

            if (typeof(TEntity).GetInterface(nameof(ISoftDeletedEntity)) == null)
                return query;

            return query.OfType<ISoftDeletedEntity>().Where(entry => !entry.Deleted).OfType<TEntity>();
        }

        #endregion

        #region Methods

        public async Task<IDbContextTransaction> BeginTransactionAsync(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.Unspecified, CancellationToken cancellationToken = default)
        {
            IDbContextTransaction dbContextTransaction = await _dataProvider.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            return dbContextTransaction;
        }




        /// <summary>
        /// Get the entity entry
        /// </summary>
        /// <param name="id">Entity entry identifier</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="ISoftDeletedEntity"/> entities)</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the entity entry
        /// </returns>
        public virtual async Task<TEntity> GetByIdAsync(int? id, Func<ICacheManager, CacheKey> getCacheKey = null, bool includeDeleted = true)
        {
            if (!id.HasValue || id == 0)
                return null;

            async Task<TEntity> getEntityAsync()
            {
                return await AddDeletedFilter(Table, includeDeleted).FirstOrDefaultAsync(entity => entity.Id == Convert.ToInt64(id));
            }

            if (getCacheKey == null)
                return await getEntityAsync();

            //caching
            var cacheKey = getCacheKey(_cacheManager)
                ?? _cacheManager.PrepareKeyForDefaultCache(CacheDefaults<TEntity>.ByIdCacheKey, id);

            return await _cacheManager.GetAsync(cacheKey, getEntityAsync);
        }

        /// <summary>
        /// Get entity entries by identifiers
        /// </summary>
        /// <param name="ids">Entity entry identifiers</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Befrosh.Core.Domain.Common.ISoftDeletedEntity"/> entities)</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the entity entries
        /// </returns>
        public virtual async Task<IList<TEntity>> GetByIdsAsync(IList<int> ids, Func<ICacheManager, CacheKey> getCacheKey = null, bool includeDeleted = true)
        {
            if (!ids?.Any() ?? true)
                return new List<TEntity>();

            async Task<IList<TEntity>> getByIdsAsync()
            {
                var query = AddDeletedFilter(Table, includeDeleted);

                //get entries
                var entries = await query.Where(entry => ids.Contains(entry.Id)).ToListAsync();

                //sort by passed identifiers
                var sortedEntries = new List<TEntity>();
                foreach (var id in ids)
                {
                    var sortedEntry = entries.Find(entry => entry.Id == id);
                    if (sortedEntry != null)
                        sortedEntries.Add(sortedEntry);
                }

                return sortedEntries;
            }

            if (getCacheKey == null)
                return await getByIdsAsync();

            //caching
            var cacheKey = getCacheKey(_cacheManager)
                ?? _cacheManager.PrepareKeyForDefaultCache(CacheDefaults<TEntity>.ByIdsCacheKey, ids);
            return await _cacheManager.GetAsync(cacheKey, getByIdsAsync);
        }


        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Befrosh.Core.Domain.Common.ISoftDeletedEntity"/> entities)</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the entity entries
        /// </returns>
        public virtual async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
            Func<ICacheManager, CacheKey> getCacheKey = null, bool includeDeleted = true)
        {
            async Task<IList<TEntity>> getAllAsync()
            {
                var query = AddDeletedFilter(Table, includeDeleted);
                query = func != null ? func(query) : query;

                return await query.ToListAsync();
            }

            return await GetEntitiesAsync(getAllAsync, getCacheKey);
        }

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Befrosh.Core.Domain.Common.ISoftDeletedEntity"/> entities)</param>
        /// <returns>Entity entries</returns>
        public virtual IList<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> func,
            Func<ICacheManager, CacheKey> getCacheKey = null, bool includeDeleted = true)
        {
            IList<TEntity> getAll()
            {
                var query = AddDeletedFilter(Table, includeDeleted);
                query = func != null ? func(query) : query;

                return query.ToList();
            }

            return GetEntities(getAll, getCacheKey);
        }

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Befrosh.Core.Domain.Common.ISoftDeletedEntity"/> entities)</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the entity entries
        /// </returns>
        public virtual async Task<IList<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null,
            Func<ICacheManager, CacheKey> getCacheKey = null, bool includeDeleted = true)
        {
            async Task<IList<TEntity>> getAllAsync()
            {
                var query = AddDeletedFilter(Table, includeDeleted);
                query = func != null ? await func(query) : query;

                return await query.ToListAsync();
            }

            return await GetEntitiesAsync(getAllAsync, getCacheKey);
        }

        /// <summary>
        /// Get all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="getCacheKey">Function to get a cache key; pass null to don't cache; return null from this function to use the default key</param>
        /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Befrosh.Core.Domain.Common.ISoftDeletedEntity"/> entities)</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the entity entries
        /// </returns>
        public virtual async Task<IList<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null,
            Func<ICacheManager, Task<CacheKey>> getCacheKey = null, bool includeDeleted = true)
        {
            async Task<IList<TEntity>> getAllAsync()
            {
                var query = AddDeletedFilter(Table, includeDeleted);
                query = func != null ? await func(query) : query;

                return await query.ToListAsync();
            }

            return await GetEntitiesAsync(getAllAsync, getCacheKey);
        }

        /// <summary>
        /// Get paged list of all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">Whether to get only the total number of entries without actually loading data</param>
        /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Befrosh.Core.Domain.Common.ISoftDeletedEntity"/> entities)</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = true)
        {
            var query = AddDeletedFilter(Table, includeDeleted);

            query = func != null ? func(query) : query;

            return await query.ToPagedListAsync((int)pageIndex, (int)pageSize, getOnlyTotalCount);
        }

        /// <summary>
        /// Get paged list of all entity entries
        /// </summary>
        /// <param name="func">Function to select entries</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">Whether to get only the total number of entries without actually loading data</param>
        /// <param name="includeDeleted">Whether to include deleted items (applies only to <see cref="Befrosh.Core.Domain.Common.ISoftDeletedEntity"/> entities)</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the paged list of entity entries
        /// </returns>
        public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = true)
        {
            var query = AddDeletedFilter(Table, includeDeleted);

            query = func != null ? await func(query) : query;

            return await query.ToPagedListAsync((int)pageIndex, (int)pageSize, getOnlyTotalCount);
        }

        /// <summary>
        /// Insert the entity entry
        /// </summary>
        /// <param name="entity">Entity entry</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertAsync(TEntity entity, bool publishEvent = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            AddDataIntegrity(entity);

            EntityEntry<TEntity> entityEntry = await _dataProvider.Set<TEntity>().AddAsync(entity).ConfigureAwait(false);
            await _dataProvider.SaveChangesAsync().ConfigureAwait(false);
            
            //event notification
            if (publishEvent)
                await _eventPublisher.EntityInsertedAsync(entity);
        }

        /// <summary>
        /// Insert entity entries
        /// </summary>
        /// <param name="entities">Entity entries</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertAsync(IList<TEntity> entities, bool publishEvent = true)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));


            AddDataIntegrity(entities);


            await _dataProvider.Set<TEntity>().AddRangeAsync(entities).ConfigureAwait(false);
            await _dataProvider.SaveChangesAsync().ConfigureAwait(false);
            

            if (!publishEvent)
                return;

            //event notification
            foreach (var entity in entities)
                await _eventPublisher.EntityInsertedAsync(entity);
        }

        /// <summary>
        /// Loads the original copy of the entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the copy of the passed entity
        /// </returns>
        public virtual async Task<TEntity> LoadOriginalCopyAsync(TEntity entity)
        {
            return await _dataProvider.Set<TEntity>()
                .FirstOrDefaultAsync(e => e.Id == Convert.ToInt32(entity.Id));
        }

        /// <summary>
        /// Update the entity entry
        /// </summary>
        /// <param name="entity">Entity entry</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateAsync(TEntity entity, bool publishEvent = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            AddDataIntegrity(entity);

            _dataProvider.Set<TEntity>().Update(entity);
            await _dataProvider.SaveChangesAsync().ConfigureAwait(false);

            //event notification
            if (publishEvent)
                await _eventPublisher.EntityUpdatedAsync(entity);
        }

        /// <summary>
        /// Update entity entries
        /// </summary>
        /// <param name="entities">Entity entries</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateAsync(IList<TEntity> entities, bool publishEvent = true)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (entities.Count == 0)
                return;

            AddDataIntegrity(entities);

            _dataProvider.Set<TEntity>().UpdateRange(entities);
            await _dataProvider.SaveChangesAsync().ConfigureAwait(false);


            //event notification
            if (!publishEvent)
                return;

            foreach (var entity in entities)
                await _eventPublisher.EntityUpdatedAsync(entity);
        }

        /// <summary>
        /// Delete the entity entry
        /// </summary>
        /// <param name="entity">Entity entry</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteAsync(TEntity entity, bool publishEvent = true)
        {
            switch (entity)
            {
                case null:
                    throw new ArgumentNullException(nameof(entity));

                case ISoftDeletedEntity softDeletedEntity:
                    softDeletedEntity.Deleted = true;
                    AddDataIntegrity(entity);
                    _dataProvider.Set<TEntity>().Update(entity);
                    break;

                default:
                    _dataProvider.Set<TEntity>().Remove(entity);
                    break;
            }
            await _dataProvider.SaveChangesAsync().ConfigureAwait(false);


            //event notification
            if (publishEvent)
                await _eventPublisher.EntityDeletedAsync(entity);
        }

        /// <summary>
        /// Delete entity entries
        /// </summary>
        /// <param name="entities">Entity entries</param>
        /// <param name="publishEvent">Whether to publish event notification</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteAsync(IList<TEntity> entities, bool publishEvent = true)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (entities.OfType<ISoftDeletedEntity>().Any())
            {
                foreach (var entity in entities)
                {
                    if (entity is ISoftDeletedEntity softDeletedEntity)
                    {
                        softDeletedEntity.Deleted = true;
                        AddDataIntegrity(entity);
                        _dataProvider.Set<TEntity>().Update(entity);
                    }
                }
                await _dataProvider.SaveChangesAsync().ConfigureAwait(false);
            }
            else
            {
                _dataProvider.Set<TEntity>().RemoveRange(entities);
                await _dataProvider.SaveChangesAsync().ConfigureAwait(false);
            }

            //event notification
            if (!publishEvent)
                return;

            foreach (var entity in entities)
                await _eventPublisher.EntityDeletedAsync(entity);
        }

        /// <summary>
        /// Delete entity entries by the passed predicate
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the number of deleted records
        /// </returns>
        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var entities = _dataProvider.Set<TEntity>().Where(predicate).ToList();
            if (entities.Count > 0)
            {
                if (entities.OfType<ISoftDeletedEntity>().Any())
                {
                    foreach (var entity in entities)
                    {
                        if (entity is ISoftDeletedEntity softDeletedEntity)
                        {
                            softDeletedEntity.Deleted = true;
                            AddDataIntegrity(entity);
                            _dataProvider.Set<TEntity>().Update(entity);
                        }
                    }
                    await _dataProvider.SaveChangesAsync().ConfigureAwait(false);
                }
                else
                {
                    _dataProvider.Set<TEntity>().RemoveRange(entities);
                    await _dataProvider.SaveChangesAsync().ConfigureAwait(false);
                }
                
            }

            return entities.Count;
        }

      

        /// <summary>
        /// Truncates database table
        /// </summary>
        /// <param name="resetIdentity">Performs reset identity column</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        //public virtual async Task TruncateAsync(bool resetIdentity = false)
        //{
        //    await _dataProvider.Set<TEntity>().TruncateAsync<TEntity>(resetIdentity);
        //}

        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table => _dataProvider.Set<TEntity>();

        #endregion
    }
}