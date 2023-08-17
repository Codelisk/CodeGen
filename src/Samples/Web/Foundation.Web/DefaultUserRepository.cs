using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorAttributes.WebAttributes.Repository;

namespace Foundation.Web
{
    [DefaultRepository]
    [UserDto]
    public class DefaultUserRepository<T, TKey> : DefaultRepository<T, TKey> where T : class
    {
        public DefaultUserRepository(BaseContext context) : base(context)
        {
        }
    }
}
