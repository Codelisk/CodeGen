using Attributes.WebAttributes.Repository;
using Foundation.Dtos.Base;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web
{
    [BaseController]
    [Route("[controller]")]
    public class BaseController<T> : Controller where T : BaseDto
    {
        protected readonly DefaultRepository<T> _repo;

        public BaseController(DefaultRepository<T> Repo)
        {
            _repo = Repo;
        }
    }
}