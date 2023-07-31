namespace Orderlyze.Service.BL.Base
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using Foundation.Web.Manager.Base;
    using Foundation.Web.Repo.Base;
    using Attributes.WebAttributes.Repository;
    using Attributes.WebAttributes.Manager;

    public abstract class CrudUserContextManager<T, TKey, TEntity, TRepo> : CrudManager<T, TKey, TEntity>
        where T : class
        where TEntity : class
        where TKey : IComparable
        where TRepo : ICrudRepository<TEntity, TKey>
    {
        //public  ISellerManager SellerManager;
        protected CrudUserContextManager(
            TRepo repository,
            IMapper mapper,
            ILogger logger
            //,ISellerManager sellerManager
            ) 
            : base( repository, mapper, logger)
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