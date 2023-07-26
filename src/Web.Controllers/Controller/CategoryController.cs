using Foundation.Dtos;
using Foundation.Web;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Controllers.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : BaseController<CategoryDto>
    {
        public CategoryController(DefaultRepository<CategoryDto> Repo) : base(Repo)
        {
        }

        [HttpPost(Name = "SaveCategory")]
        public Task<CategoryDto> SaveCategory(CategoryDto categoryDto)
        {
            return _repo.Save(categoryDto);
        }
        [HttpGet(Name = "GetCategories")]
        public Task<List<CategoryDto>> GetCategories()
        {
            return _repo.GetAll();
        }
    }
}
