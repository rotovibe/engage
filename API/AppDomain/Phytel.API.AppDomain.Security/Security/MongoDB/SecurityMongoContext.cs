using MongoDB.Bson;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.Security
{
    public class SecurityMongoContext : MongoContext
    {
        private static string dbServiceName = "APISecurity";
        private static string COLL_APIKEYS = "APIKeys";
        private static string COLL_APIUSERS = "APIUsers";
        private static string COLL_APISESSIONS = "APISessions";

        public SecurityMongoContext() : base(dbServiceName, false)
		{
            APIUsers = new MongoSet<MEAPIUser, ObjectId>(this, COLL_APIUSERS);
            APIKeys = new MongoSet<MEAPIKey, ObjectId>(this, COLL_APIKEYS);
            APISessions = new MongoSet<MEAPISession, ObjectId>(this, COLL_APISESSIONS);
		}

        public MongoSet<MEAPIUser, ObjectId> APIUsers {get;private set;}
        public MongoSet<MEAPIKey, ObjectId> APIKeys { get; private set; }
        public MongoSet<MEAPISession, ObjectId> APISessions { get; private set; }
    }
}
