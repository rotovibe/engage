using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Atmosphere.Core
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
