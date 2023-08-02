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
    public class CategoryContext : BaseContext
    {
        public CategoryContext(DbContextOptions<BaseContext> options)
        : base(options)
        {
        }
    }
}
