using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorAttributes.WebAttributes.Manager;

namespace Foundation.Web
{
    [DefaultManager]
    [UserDto]
    public class DefaultUserManager<TDto, TKey, TEntity> : DefaultManager<TDto, TKey, TEntity> where TDto : class where TEntity : class
    {
        public DefaultUserManager(IDefaultRepository<TEntity, TKey> repo, IMapper mapper) : base(repo, mapper)
        {
        }
    }
}
