using System.Collections.Generic;
using System.Linq;

namespace Phytel.Services.Utility
{
    public static class QueryableExtend
    {
        /// <summary>
        /// Retries enumerating the list for the provided number of retries over the provided retry interval (delay). This should be used when enumerating a
        /// queryable from a EF DbSet or Mongo MongoSet rather than the standard ToList.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="retries">The number of attempts to retry enumerating the list</param>
        /// <param name="retryDelay">The number of milliseconds to wait between retries</param>
        /// <returns>Returns the enumerated list once able to successfully enumerate the list.</returns>
        public static List<T> ToListWithRetry<T>(this IQueryable<T> queryable, int retries = RetryHelper.RETRIES, int retryDelay = RetryHelper.RETRYDELAY)
        {
            return RetryHelper.DoWithRetry<List<T>>(() =>
            {
                return queryable.ToList();
            },
                retries,
                retryDelay
            );
        }
    }
}