namespace Orderlyze.Service.DL.Base
{
    using System;
    using System.Threading.Tasks;
    using Foundation.Web.Repo.Base;
    using Microsoft.EntityFrameworkCore.Storage;
    public class Transaction : ITransaction
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public Transaction(IUnitOfWork unitOfWork, IDbContextTransaction dbTran)
        {
            UnitOfWork = unitOfWork;
            _dbTran    = dbTran;
        }

        #region Transaction

        private IDbContextTransaction _dbTran;

        public bool InTransaction => _dbTran != null;

        private void CheckInTransaction()
        {
            if (InTransaction == false)
            {
                throw new ArgumentException("Transaction not started");
            }
        }

        [Obsolete("This Method is Deprecated")]
        public async Task SaveChangesAsync()
        {
            CheckInTransaction();
            await UnitOfWork.SaveChangesAsync();
        }

        [Obsolete("This Method is Deprecated")]
        public async Task CommitTransactionAsync()
        {
            await SaveChangesAsync();
            await _dbTran.CommitAsync();
            _dbTran = null;
        }

        public void RollbackTransaction()
        {
            CheckInTransaction();

            _dbTran.Rollback();
            _dbTran = null;
        }

        #endregion

        #region dispose

        public void Dispose()
        {
            if (InTransaction)
            {
                // if commit is not called, rollback transaction now
                RollbackTransaction();
            }
        }

        #endregion
    }
}