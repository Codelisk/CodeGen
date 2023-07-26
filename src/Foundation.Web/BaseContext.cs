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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase(nameof(T));
        }
        public BaseContext(DbContextOptions<BaseContext<T>> opt)
        : base(opt)
        {
        }
        public DbSet<T> Items { get; set; } = null!;
    }
}
