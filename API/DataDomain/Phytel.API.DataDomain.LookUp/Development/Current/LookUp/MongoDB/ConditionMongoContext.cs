using MongoDB.Bson;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.LookUp
{
    public class ConditionMongoContext : MongoContext
    {
        private static string COLL_CONDITIONS = "Condition";

        public ConditionMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            Conditions = new MongoSet<MECondition, ObjectId>(this, COLL_CONDITIONS);
		}

        public MongoSet<MECondition, ObjectId> Conditions { get; private set; }
    }
}
