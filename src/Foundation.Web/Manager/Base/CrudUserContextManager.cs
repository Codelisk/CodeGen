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
    using Attributes.WebAttributes.Dto;
    using Foundation.Web.Repo;

    public abstract class CrudUserContextManager<T, TKey, TEntity> : CrudManager<T, TKey, TEntity>
        where T : class
        where TEntity : class
        where TKey : IComparable
    {
        //public  ISellerManager SellerManager;
        protected CrudUserContextManager(
            CrudRepository<TEntity, TKey> repository,
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