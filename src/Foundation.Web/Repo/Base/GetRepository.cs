namespace Orderlyze.Service.DL.Base
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public abstract class GetRepository<TEntity, TKey> : DbRepositoryBase<TEntity> where TEntity : class
    {
        protected GetRepository(DbContextBase<TEntity> dbContext)
            : base(dbContext)
        {
        }

        #region QueryProperties

        protected virtual IQueryable<TEntity> QueryWithInclude => AddInclude(Query);

        protected virtual IQueryable<TEntity> QueryWithOptional => AddOptionalWhere(Query);

        protected virtual FilterBuilder<TEntity, TKey> FilterBuilder { get; } = null;

        #endregion

        #region Get

        protected async Task<IList<TEntity>> GetAll(IQueryable<TEntity> query)
        {
            return await query.ToListAsync();
        }

        protected async Task<TEntity> Get(IQueryable<TEntity> query, TKey key)
        {
            return await AddPrimaryWhere(query, key).FirstOrDefaultAsync();
        }

        protected async Task<IList<TEntity>> Get(IQueryable<TEntity> query, IEnumerable<TKey> keys)
        {
            return await AddPrimaryWhereIn(query, keys).ToListAsync();
        }

        public async Task<IList<TEntity>> GetAll()
        {
            return await GetAll(AddInclude(QueryWithOptional));
        }

        public async Task<int> GetCount()
        {
            return await AddInclude(QueryWithOptional).CountAsync();
        }

        public async Task<TEntity> Get(TKey key)
        {
            return await Get(QueryWithInclude, key);
        }

        public async Task<IList<TEntity>> Get(IEnumerable<TKey> keys)
        {
            return await Get(QueryWithInclude, keys);
        }

        #endregion

        #region overrides

        protected virtual IQueryable<TEntity> AddOptionalWhere(IQueryable<TEntity> query) => query;

        protected virtual IQueryable<TEntity> AddInclude(IQueryable<TEntity> query) => query;

        protected virtual IQueryable<TEntity> AddPrimaryWhere(IQueryable<TEntity> query, TKey key)
        {
            return FilterBuilder.PrimaryWhere(query, key);
        }

        protected virtual IQueryable<TEntity> AddPrimaryWhereIn(IQueryable<TEntity> query, IEnumerable<TKey> keys)
        {
            return FilterBuilder.PrimaryWhereIn(query, keys);
        }

        #endregion
    }
}