using Foundation.Web.Repo.Base;

namespace Orderlyze.Service.DL.Base
{
    public class UnitOfWorkContainer
    {
        public IUnitOfWork UnitOfWork { get; set; }

        protected UnitOfWorkContainer(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }

    public class UnitOfWorkContainer<TUnitOfWork> : UnitOfWorkContainer where TUnitOfWork : IUnitOfWork
    {
        public UnitOfWorkContainer(TUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}