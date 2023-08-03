using Foundation.Web.Repo.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web.Repo
{
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