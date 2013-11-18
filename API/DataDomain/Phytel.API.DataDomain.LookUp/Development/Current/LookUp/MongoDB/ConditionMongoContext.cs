using MongoDB.Bson;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.LookUp
{
    public class ProblemMongoContext : MongoContext
    {
        private static string COLL_PROBLEMLOOKUP = "ProblemLookUp";

        public ProblemMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            Problems = new MongoSet<MEProblem, ObjectId>(this, COLL_PROBLEMLOOKUP);
		}

        public MongoSet<MEProblem, ObjectId> Problems { get; private set; }
    }
}
