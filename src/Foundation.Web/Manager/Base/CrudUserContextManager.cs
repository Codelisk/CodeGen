namespace Orderlyze.Service.BL.Base
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using Foundation.Web.Manager.Base;
    using Foundation.Web.Repo.Base;

    public abstract class CrudUserContextManager<T, TKey, TEntity, TRepo> : CrudManager<T, TKey, TEntity,TRepo>
        where T : class
        where TEntity : class
        where TKey : IComparable
        where TRepo : ICrudRepository<TEntity, TKey>
    {
        //public  ISellerManager SellerManager;
        protected CrudUserContextManager(IUnitOfWork unitOfWork,
            TRepo repository,
            IMapper mapper,
            ILogger logger
            //,ISellerManager sellerManager
            ) 
            : base(unitOfWork, repository, mapper, logger)
        {
            //SellerManager = sellerManager;
        }
        // Todo ? 
        //protected override async Task Modified()
        //{
        //    await SellerManager.SetLastSellerBaseDataDateTime();
        //}

    }
}