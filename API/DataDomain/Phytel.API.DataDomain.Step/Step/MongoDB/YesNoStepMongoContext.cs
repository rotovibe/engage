using MongoDB.Bson;
using Phytel.API.DataDomain.Step.DTO;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.Step
{
    public class YesNoStepMongoContext : MongoContext
    {
        private static string COLL_Step = "Step";

        public YesNoStepMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
            YesNoSteps = new MongoSet<MEYesNo, ObjectId>(this, COLL_Step);
		}

        public MongoSet<MEYesNo, ObjectId> YesNoSteps { get; private set; }
    }
}
