using MongoDB.Bson;
using Phytel.API.DataDomain.Template.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.Template
{
    public class TemplateMongoContext : MongoContext
    {
        private static string COLL_TemplateS = "Template";

        public TemplateMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            Templates = new MongoSet<METemplate, ObjectId>(this, COLL_TemplateS);
		}

        public MongoSet<METemplate, ObjectId> Templates { get; private set; }
    }
}
