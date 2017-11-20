using System.Collections.Generic;
using System.Linq;

namespace Store.Services
{
    public static class FilteringHelpers
    {
        public static List<TEntity> Paginate<TEntity>(this IQueryable<TEntity> entities, int pageNumber, int pageSize)
            where TEntity : class
        {
            return entities.
               Skip((pageNumber - 1) * pageSize).
               Take(pageSize).
               ToList();
        }
    }
}
