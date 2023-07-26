using Attributes.WebAttributes.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web
{
    [BaseContext]
    public class BaseContext<T> : DbContext where T : class
    {
        public BaseContext(DbContextOptions<BaseContext<T>> options)
        : base(options)
        {
        }
        public DbSet<T> Items { get; set; } = null!;
    }
}
