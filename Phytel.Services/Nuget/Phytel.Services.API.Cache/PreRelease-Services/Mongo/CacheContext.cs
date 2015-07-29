using MongoDB.Bson;
using Phytel.Services.Mongo.Linq;

namespace Phytel.Services.API.Cache.Mongo
{
    public class CacheContext : MongoContext
    {
        public CacheContext(string connectionName, bool isContract)
            : base(connectionName, isContract)
        {
            Cache = new MongoSet<CacheMongoEntity, ObjectId>(this);
        }

        public MongoSet<CacheMongoEntity, ObjectId> Cache { get; private set; }
    }
}
