using Foundation.Dtos.Base;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web
{
    public class BaseController<T> : Controller where T : BaseDto
    {
        protected readonly DefaultRepository<T> _repo;

        public BaseController(DefaultRepository<T> Repo)
        {
            _repo = Repo;
        }
    }
}