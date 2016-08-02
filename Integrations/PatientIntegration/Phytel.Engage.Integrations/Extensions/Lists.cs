using System.Collections.Generic;

namespace Phytel.Engage.Integrations.Extensions
{
    public static class Lists
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> collection, int batchSize)
        {
            var nextbatch = new List<T>(batchSize);
            foreach (var item in collection)
            {
                nextbatch.Add(item);
                if (nextbatch.Count == batchSize)
                {
                    yield return nextbatch;
                    nextbatch = new List<T>();
                }
            }

            if (nextbatch.Count > 0)
                yield return nextbatch;
        }

        public static int Pages<T>(this IEnumerable<T> collection, int batchSize)
        {
            var c = collection as ICollection<T>;
            if (c != null)
                return c.Count/ batchSize;

            var result = 0;
            using (var enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    result++;
            }
            return result / batchSize;
        }    
    }
}
