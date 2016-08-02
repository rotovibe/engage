using System.Collections.Generic;
using System.Linq;

namespace Phytel.API.Common.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Checks if a List is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }

            return !enumerable.Any();
        }

    }
}
