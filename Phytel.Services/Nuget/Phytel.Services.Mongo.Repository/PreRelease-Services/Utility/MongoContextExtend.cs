using Phytel.Services.Mongo.Linq;
using System;
using System.Linq;
using System.Reflection;

namespace Phytel.Services.Mongo.Repository
{
    public static class MongoContextExt
    {
        public static MongoSet<TEntity, TKey> Set<TEntity, TKey>(this MongoContext mongoContext)
            where TEntity : IMongoEntity<TKey>
        {
            MongoSet<TEntity, TKey> rvalue = null;

            Type mongoContextType = mongoContext.GetType();
            PropertyInfo[] properties = mongoContextType.GetProperties();

            PropertyInfo rvalueProperty = properties.DefaultIfEmpty(null).FirstOrDefault(x => x.PropertyType.FullName == typeof(MongoSet<TEntity, TKey>).FullName);
            object rvalueValue = rvalueProperty.GetValue(mongoContext, null);

            rvalue = rvalueValue as MongoSet<TEntity, TKey>;

            return rvalue;
        }
    }
}