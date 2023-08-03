using Attributes.WebAttributes.Dto;
using Attributes.WebAttributes.Manager;

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
