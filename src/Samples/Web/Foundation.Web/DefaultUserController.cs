using Codelisk.GeneratorAttributes.WebAttributes.Controller;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Foundation.Dtos.Base;

namespace Foundation.Web
{
    [DefaultController]
    [UserDto]
    public class DefaultUserController<T, TKey, TEntity> : DefaultController<T, TKey, TEntity>
        where T : BaseDto
        where TEntity : class
        where TKey : IComparable
    {
        public DefaultUserController(IDefaultManager<T, TKey, TEntity> manager) : base(manager)
        {
        }
    }
}
