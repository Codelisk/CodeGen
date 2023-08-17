using Codelisk.GeneratorAttributes.WebAttributes.Database;
using Microsoft.EntityFrameworkCore;

namespace Foundation.Web
{
    [BaseContext]
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
