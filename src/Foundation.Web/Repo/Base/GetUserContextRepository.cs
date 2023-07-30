namespace Orderlyze.Service.DL.Base
{
    using System.Linq;
    using Foundation.Web.Shared;

    public abstract class GetUserContextRepository<TDbContext, TEntity, TKey> : GetRepository<TDbContext, TEntity, TKey>
        where TDbContext : DbContextBase where TEntity : SellerBase
    {
        protected IUserContext UserContext;
        protected GetUserContextRepository(TDbContext dbContext, IUserContext userContext)
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