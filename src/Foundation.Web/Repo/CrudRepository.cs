using Orderlyze.Service.DL.Base;

namespace Foundation.Web.Repo
{
    using Attributes.WebAttributes.Repository;
    using Foundation.Web.Repo.Base;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class CrudRepository<TEntity, TKey> : GetRepository<TEntity, TKey> where TEntity : class
    {
        protected CrudRepository(DbContextBase dbContext)
            : base(dbContext)
        {
        }

        protected virtual IQueryable<TEntity> TrackingQueryWithInclude => AddInclude(TrackingQuery);

        protected virtual IQueryable<TEntity> TrackingQueryWithOptional => AddOptionalWhere(TrackingQuery);


        #region Get Tracking

        public async Task<IList<TEntity>> GetTrackingAll()
        {
            return await GetAll(AddInclude(TrackingQueryWithOptional));
        }

        public async Task<TEntity> GetTracking(TKey key)
        {
            return await Get(TrackingQueryWithInclude, key);
        }

        public async Task<TEntity> GetTrackingWithoutInclude(TKey key)
        {
            return await Get(TrackingQuery, key);
        }

        public async Task<IList<TEntity>> GetTracking(IEnumerable<TKey> keys)
        {
            return await Get(TrackingQueryWithInclude, keys);
        }

        #endregion

        #region CRUD

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            DeleteEntities(entities);
        }

        public void SetState(TEntity entity, MyEntityState state)
        {
            SetEntityState(entity, (Microsoft.EntityFrameworkCore.EntityState)state);
        }

        public void SetValue(TEntity entity, TEntity values)
        {
            AssignValues(entity, values);
            base.SetValue(entity, values);
        }

        public void SetValueGraph(TEntity trackingEntity, TEntity values)
        {
            AssignValuesGraph(trackingEntity, values);
        }

        protected virtual void AssignValues(TEntity trackingEntity, TEntity values)
        {
        }

        protected virtual void AssignValuesGraph(TEntity trackingEntity, TEntity values)
        {
            SetValue(trackingEntity, values);
        }

        #endregion
    }
}