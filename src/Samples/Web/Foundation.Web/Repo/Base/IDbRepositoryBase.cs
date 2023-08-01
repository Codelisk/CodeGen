namespace Foundation.Web.Repo.Base
{
    using Foundation.Web.Repo;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IDbRepositoryBase<in TModel> : IRepository where TModel : class
    {
        void Add(TModel entity);
        void AddRange(IEnumerable<TModel> entities);
        void Delete(TModel entity);
        Task SaveChangesAsync();
    }
}