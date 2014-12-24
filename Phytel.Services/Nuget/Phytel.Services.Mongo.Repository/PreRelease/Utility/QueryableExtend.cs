using Phytel.Services.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.Mongo.Repository
{
    public static class QueryableExtend
    {
        public static List<T> ToListWithRetry<T>(this IQueryable<T> queryable)
        {
            return RetryHelper.DoWithRetry<List<T>>(() => 
            {
                return queryable.ToList();
            }, 
                RetryHelper.RETRIES, 
                RetryHelper.RETRYDELAY
            );
        }
    }
}
