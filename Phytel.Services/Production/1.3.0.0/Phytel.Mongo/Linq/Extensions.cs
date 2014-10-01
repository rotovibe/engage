using System.Collections.Generic;
using System.Linq;

namespace Phytel.Mongo.Linq
{
    public static class Extensions
    {
        /// <summary>
        /// Returns if the collection is not null and has items
        /// </summary>
        /// <typeparam name="T">Type of the collection</typeparam>
        /// <returns></returns>
        public static bool HasItems<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
            {
                return false;
            }

            return collection.Any();
        }
    }
}
