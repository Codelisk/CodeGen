namespace Orderlyze.Service.DL.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FilterBuilder<TEntity, TKey> where TEntity : class
    {
        public Func<IQueryable<TEntity>, TKey, IQueryable<TEntity>>              PrimaryWhere   { get; set; }
        public Func<IQueryable<TEntity>, IEnumerable<TKey>, IQueryable<TEntity>> PrimaryWhereIn { get; set; }
    }
}