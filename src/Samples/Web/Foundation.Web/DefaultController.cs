using Codelisk.GeneratorAttributes.WebAttributes.Controller;
using Foundation.Dtos.Base;

namespace Foundation.Web
{
    [DefaultController]
    [Route("[controller]")]
    public class DefaultController<T, TKey, TEntity> : Microsoft.AspNetCore.Mvc.Controller
        where T : BaseDto
        where TEntity : class
        where TKey : IComparable
    {
        protected readonly IDefaultManager<T, TKey, TEntity> _manager;

        public DefaultController(IDefaultManager<T, TKey, TEntity> manager)
        {
            _manager = manager;
        }
    }
}