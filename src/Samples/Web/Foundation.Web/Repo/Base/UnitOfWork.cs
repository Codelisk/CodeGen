namespace Orderlyze.Service.DL.Base
{

    using System;
    using System.Threading.Tasks;
    using Foundation.Web.Repo.Base;
    using Framework.Repository;
    using Microsoft.EntityFrameworkCore;


    public class UnitOfWork<T> : IUnitOfWork
        where T : DbContext
    {
        public T Context { get; private set; }

        public UnitOfWork(T context)
        {
            Context = context;
            Context.Database.CreateExecutionStrategy();
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        //public async Task<int> ExecuteSqlCommand(string sql)
        //{
        //    return await Context.Database.ExecuteSqlRawAsync(sql);
        //}

        //public async Task<int> ExecuteSqlCommand(string sql, params object[] parameters)
        //{
        //    return await Context.Database.ExecuteSqlRawAsync(sql, parameters);
        //}

        #region Transaction

        public bool IsInTransaction()
        {
            return Context.Database.CurrentTransaction != null;
        }

        [Obsolete("This Method is Deprecated")]
        public ITransaction BeginTransaction()
        {
            if (IsInTransaction())
            {
                throw new ArgumentException("Nested transaction are not supported.");
            }
            Context.Database.CreateExecutionStrategy();
            return new Transaction(this, Context.Database.BeginTransaction());
        }

        public Task<int> ExecuteSqlCommand(string sql)
        {
            throw new NotImplementedException();
        }

        public Task<int> ExecuteSqlCommand(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}