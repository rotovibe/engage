using MongoDB.Bson;
using Phytel.API.DataDomain.Step.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.Step
{
    public class TextMongoContext : MongoContext
    {
        private static string COLL_Texts = "TextStep";

        public TextMongoContext(string contractDBName)
            : base(contractDBName, true)
        {
            TextSteps = new MongoSet<METext, ObjectId>(this, COLL_Texts);
        }

        public MongoSet<METext, ObjectId> TextSteps { get; private set; }
    }
}
