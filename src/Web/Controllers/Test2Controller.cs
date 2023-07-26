using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class Test2Controller : ControllerBase
    {
        public Test2Controller()
            : base()
        {
        }

        [HttpGet("GetCategories2")]
        public async Task<string> GetAllCategories()
        {
            return "";
        }
    }
}