using Foundation.Dtos;
using Foundation.Web;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Database.Models
{
    public class CategoryContext : BaseContext<CategoryDto>
    {
        public CategoryContext(DbContextOptions<BaseContext<CategoryDto>> options)
        : base(options)
        {
        }
    }
}
