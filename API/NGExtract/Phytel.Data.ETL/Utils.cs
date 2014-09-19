using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

namespace Phytel.Data.ETL
{
    public static class Utils
    {
        public static List<T> GetMongoCollectionList<T>(MongoCollection<T> mogoList, int skip)
        {
            var list = new List<T>();
            var count = mogoList.Count();
            var interval = skip;
            for (var i = 0; i <= count; i = i + interval)
            {
                list.AddRange(
                    mogoList.FindAllAs<T>()
                        .Skip(i)
                        .Take(interval)
                        .ToList());
            }
            return list;
        }
    }
}
