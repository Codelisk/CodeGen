namespace Orderlyze.Service.DL.Base
{
    using Microsoft.EntityFrameworkCore;

    public abstract class DbContextBase<T> : DbContext where T : class
    {
        protected DbContextBase(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {        
        }
    }
}