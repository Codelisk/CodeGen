using Attributes.WebAttributes.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web
{
    public partial class BaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("TEST");
        }
        public BaseContext(DbContextOptions<BaseContext> opt)
        : base(opt)
        {
        }
    }
}
