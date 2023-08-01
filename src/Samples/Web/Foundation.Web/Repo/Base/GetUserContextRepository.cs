namespace Orderlyze.Service.DL.Base
{
    using System.Linq;
    using Foundation.Web.Repo;
    using Foundation.Web.Shared;

    public abstract class GetUserContextRepository<TEntity, TKey> : GetRepository<TEntity, TKey>
        where TEntity : SellerBase
    {
        protected IUserContext UserContext;
        protected GetUserContextRepository(DbContextBase dbContext, IUserContext userContext)
            : base(dbContext)
        {
            UserContext = userContext;
        }

        #region QueryProperties

        protected override  IQueryable<TEntity> QueryWithInclude => AddInclude(QueryWithUserContext);

        protected override IQueryable<TEntity> QueryWithOptional => AddOptionalWhere(QueryWithUserContext);

        protected IQueryable<TEntity> QueryWithUserContext => Query.Where(x => x.SellerId == UserContext.SellerId);


        #endregion

    }
}