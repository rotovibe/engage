using MongoDB.Bson;
using Phytel.API.DataDomain.Template.DTO;
using Phytel.Services.Mongo.Linq;

namespace DataDomain.Template.Repo
{
	public class TemplateMongoContext : MongoContext
	{
		private static string COLL_TemplateS = "Template";
		public string ContractName { get; set; }

		public TemplateMongoContext(string contractDBName)
			: base(contractDBName, true)
		{
			ContractName = contractDBName;
			Templates = new MongoSet<METemplate, ObjectId>(this, COLL_TemplateS);
		}

		public MongoSet<METemplate, ObjectId> Templates { get; private set; }
	}
}
