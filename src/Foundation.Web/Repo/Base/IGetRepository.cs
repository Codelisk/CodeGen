namespace Foundation.Web.Repo.Base
{
    using Foundation.Web.Repo;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IGetRepository<TEntity, TKey> : IRepository
        where TEntity : class
    {
        Task<IList<TEntity>> GetAll();

        Task<TEntity> Get(TKey key);

        Task<IList<TEntity>> Get(IEnumerable<TKey> keys);

        Task<int> GetCount();
    }
}