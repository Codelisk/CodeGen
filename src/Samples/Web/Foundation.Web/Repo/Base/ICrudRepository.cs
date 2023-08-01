namespace Foundation.Web.Repo.Base
{
    using Foundation.Web.Repo;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICrudRepository<TEntity, TKey> : IGetRepository<TEntity, TKey>
        where TEntity : class
    {
        Task<IList<TEntity>> GetTrackingAll();

        Task<TEntity> GetTracking(TKey key);


        Task<IList<TEntity>> GetTracking(IEnumerable<TKey> keys);

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);

        void SetValue(TEntity trackingEntity, TEntity values);

        void SetValueGraph(TEntity trackingEntity, TEntity values);

        void SetState(TEntity entity, MyEntityState state);

        Task SaveChangesAsync();
    }
}