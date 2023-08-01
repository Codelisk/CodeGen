namespace Foundation.Web.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ListExtensions
    {
        public static ICollection<T> ToICollection<T>(this IEnumerable<T> list)
        {
            var collection = list as ICollection<T>;
            return collection ?? list.ToList();
        }
    }
}
