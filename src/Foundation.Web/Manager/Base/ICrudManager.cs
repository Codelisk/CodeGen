namespace Foundation.Web.Manager.Base
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.JsonPatch;

    public interface ICrudManager<T, TId> : IGetManager<T, TId> where T : class
    {
        Task<TId> Add(T value);
        Task<T> Set(T value);
        Task Update(T value);
        Task Delete(T value);
        Task Delete(TId key);
        Task Patch(TId key, JsonPatchDocument<T> patch);
        Task<IEnumerable<TId>> Add(IEnumerable<T> values);
        Task Update(IEnumerable<T> values);
        Task Delete(IEnumerable<T> values);
        Task Delete(IEnumerable<TId> keys);
    }
}