using MongoDB.Bson;
using Phytel.API.DataDomain.Step.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.Step
{
    public class YesNoStepMongoContext : MongoContext
    {
        private static string COLL_Step = "Step";

        public YesNoStepMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            YesNoSteps = new MongoSet<MEYesNo, ObjectId>(this, COLL_Step);
		}

        public MongoSet<MEYesNo, ObjectId> YesNoSteps { get; private set; }
    }
}
