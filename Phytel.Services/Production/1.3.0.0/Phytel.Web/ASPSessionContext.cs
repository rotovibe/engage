using System.Configuration;
using Phytel.Mongo.Linq;
using Phytel.Web.Cache;
using MongoDB.Bson;

namespace Phytel.Web
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
