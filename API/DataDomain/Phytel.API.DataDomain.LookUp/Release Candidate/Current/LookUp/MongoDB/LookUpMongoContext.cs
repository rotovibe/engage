using MongoDB.Bson;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.LookUp
{
    public class LookUpMongoContext : MongoContext
    {
        private static string COLL_LOOKUP = "LookUp";

        public LookUpMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            LookUps = new MongoSet<MELookup, ObjectId>(this, COLL_LOOKUP);
		}

        public MongoSet<MELookup, ObjectId> LookUps { get; private set; }
    }
}
