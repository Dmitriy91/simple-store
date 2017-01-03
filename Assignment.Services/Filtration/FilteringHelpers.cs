using System.Collections.Generic;
using System.Linq;

namespace Assignment.Services
{
    public static class FilteringHelpers
    {
        public static IEnumerable<TEntity> Paginate<TEntity>(this IQueryable<TEntity> entities, int pageNumber, int pageSize)
            where TEntity : class
        {
             return entities.
                Skip((pageNumber - 1) * pageSize).
                Take(pageSize).
                ToList();
        }
    }
}
