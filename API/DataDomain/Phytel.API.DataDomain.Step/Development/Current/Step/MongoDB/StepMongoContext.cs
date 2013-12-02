using MongoDB.Bson;
using Phytel.API.DataDomain.Step.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.Step
{
    public class StepMongoContext : MongoContext
    {
        private static string COLL_StepS = "Step";

        public StepMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            Steps = new MongoSet<MEStep, ObjectId>(this, COLL_StepS);
		}

        public MongoSet<MEStep, ObjectId> Steps { get; private set; }
    }
}
