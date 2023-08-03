using Attributes.WebAttributes.Dto;
using Attributes.WebAttributes.Repository;

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
