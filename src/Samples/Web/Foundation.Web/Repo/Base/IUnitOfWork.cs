namespace Foundation.Web.Repo.Base
{
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        // SQL Commands

        Task<int> ExecuteSqlCommand(string sql);
        Task<int> ExecuteSqlCommand(string sql, params object[] parameters);

        // Transaction

        ITransaction BeginTransaction();
    }
}