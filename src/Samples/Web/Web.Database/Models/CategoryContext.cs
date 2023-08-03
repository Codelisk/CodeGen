using Foundation.Web;
using Microsoft.EntityFrameworkCore;

namespace Web.Database.Models
{
    public class CategoryContext : BaseContext
    {
        public CategoryContext(DbContextOptions<BaseContext> options)
        : base(options)
        {
        }
    }
}
