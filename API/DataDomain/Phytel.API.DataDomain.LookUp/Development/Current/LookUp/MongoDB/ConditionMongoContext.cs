using MongoDB.Bson;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.LookUp
{
    public class ConditionMongoContext : MongoContext
    {
        private static string COLL_PROBLEMLOOKUP = "ProblemLookUp";

        public ConditionMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            Conditions = new MongoSet<MEProblem, ObjectId>(this, COLL_PROBLEMLOOKUP);
		}

        public MongoSet<MEProblem, ObjectId> Conditions { get; private set; }
    }
}
