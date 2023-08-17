using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;

namespace Foundation.Web.Interfaces
{
    public interface IDefaultRepository<TEntity, TKey> where TEntity : class
    {
        [Delete]
        Task Delete(TEntity t);
        [Get]
        Task<TEntity> Get(TKey id);
        [GetAll]
        Task<List<TEntity>> GetAll();
        [Save]
        Task<TEntity> Save(TEntity t);
    }
}
