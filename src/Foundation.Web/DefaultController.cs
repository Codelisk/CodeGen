using Attributes.WebAttributes.Repository;
using Foundation.Dtos.Base;
using Foundation.Web.Manager.Base;
using Foundation.Web.Repo.Base;
using Microsoft.AspNetCore.Mvc;
using Orderlyze.Service.BL;

namespace Foundation.Web
{
    [DefaultController]
    [Route("[controller]")]
    public class DefaultController<T, TKey, TEntity> : Microsoft.AspNetCore.Mvc.Controller
        where T : BaseDto
        where TEntity : class
        where TKey : IComparable
    {
        protected readonly DefaultManager<T, TKey, TEntity> _manager;

        public DefaultController(DefaultManager<T, TKey, TEntity> manager)
        {
            _manager = manager;
        }
    }
}