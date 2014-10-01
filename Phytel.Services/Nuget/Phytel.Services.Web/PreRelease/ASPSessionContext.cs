using MongoDB.Bson;
using Phytel.Services.Mongo.Linq;
using Phytel.Services.Web.Cache;

namespace Phytel.Services.Web
{
    [MongoDatabase(DatabaseName="ASPSessionState")]
    public class ASPSessionContext : MongoContext
    {
        public ASPSessionContext() : base("ASPSessionState", false)
        {
            Cache = new MongoSet<MongoCacheItem, ObjectId>(this);
        }

        public MongoSet<MongoCacheItem, ObjectId> Cache { get; private set; }
    }
}
