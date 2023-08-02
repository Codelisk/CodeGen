﻿using System.Collections.Generic;

namespace Foundation.Web.Repo
{
    using System.Linq;
    using Attributes.WebAttributes.Dto;
    using Attributes.WebAttributes.Repository;
    using Foundation.Web.Shared;

    public abstract class CrudUserContextRepository<TEntity, TKey> : CrudRepository<TEntity, TKey>
        where TEntity : SellerBase
    {
        protected IUserContext UserContext;
        protected CrudUserContextRepository(DbContextBase dbContext, IUserContext userContext)
            : base(dbContext)
        {
            UserContext = userContext;
        }

        protected override IQueryable<TEntity> TrackingQueryWithInclude => AddInclude(TrackingQueryWithUserContext);

        protected override IQueryable<TEntity> TrackingQueryWithOptional => AddOptionalWhere(TrackingQueryWithUserContext);

        protected override IQueryable<TEntity> QueryWithInclude => AddInclude(QueryWithUserContext);

        protected override IQueryable<TEntity> QueryWithOptional => AddOptionalWhere(QueryWithUserContext);
        protected IQueryable<TEntity> QueryWithUserContext => Query.Where(x => x.SellerId == UserContext.SellerId);
        protected IQueryable<TEntity> TrackingQueryWithUserContext => TrackingQuery.Where(x => x.SellerId == UserContext.SellerId);

        public override void AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        public override void Add(TEntity entity)
        {
            if (entity.SellerId == 0 && UserContext.SellerId != 0)
            {
                entity.SellerId = UserContext.SellerId;
            }

            base.Add(entity);
        }

        protected override void AssignValuesGraph(TEntity trackingEntity, TEntity values)
        {
            if (values.SellerId == 0 && UserContext.SellerId != 0)
            {
                values.SellerId = UserContext.SellerId;
            }

            base.AssignValuesGraph(trackingEntity, values);
        }
    }
}