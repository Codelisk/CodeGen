namespace Foundation.Web.Manager.Base
{
    using Foundation.Web.Manager;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IGetManager<T, in TId> : IManager where T : class
    {
        Task<T> Get(TId id);

        Task<IList<T>> Get(IEnumerable<TId> keys);
        Task<IList<T>> GetAll();
        Task<int> GetCount();
    }
}