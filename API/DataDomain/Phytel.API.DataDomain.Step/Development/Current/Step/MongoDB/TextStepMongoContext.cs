using MongoDB.Bson;
using Phytel.API.DataDomain.Step.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.Step
{
    public class TextStepMongoContext : MongoContext
    {
        private static string COLL_Step = "Step";

        public TextStepMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            TextSteps = new MongoSet<METext, ObjectId>(this, COLL_Step);
		}

        public MongoSet<METext, ObjectId> TextSteps { get; private set; }
    }
}
