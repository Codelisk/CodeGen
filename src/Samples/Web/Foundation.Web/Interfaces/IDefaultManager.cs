using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;

namespace Foundation.Web.Interfaces
{
    public interface IDefaultManager<TDto, TKey, TEntity> where TDto : class where TEntity : class
    {
        [Delete]
        Task Delete(TDto t);
        [Get]
        Task<TDto> Get(TKey id);
        [GetAll]
        Task<List<TDto>> GetAll();
        [Save]
        Task<TDto> Save(TDto t);
    }
}
