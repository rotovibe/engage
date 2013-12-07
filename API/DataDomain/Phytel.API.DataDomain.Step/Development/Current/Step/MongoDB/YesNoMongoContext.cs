using MongoDB.Bson;
using Phytel.API.DataDomain.Step.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.Step
{
    public class YesNoMongoContext : MongoContext
    {
        private static string COLL_YesNos = "YesNoStep";

        public YesNoMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            YesNoSteps = new MongoSet<MEYesNo, ObjectId>(this, COLL_YesNos);
		}

        public MongoSet<MEYesNo, ObjectId> YesNoSteps { get; private set; }
    }
}
