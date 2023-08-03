namespace Foundation.Web.Repo
{
    using Attributes.WebAttributes.Database;
    using Microsoft.EntityFrameworkCore;

    [BaseContext]
    public abstract class DbContextBase : DbContext
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