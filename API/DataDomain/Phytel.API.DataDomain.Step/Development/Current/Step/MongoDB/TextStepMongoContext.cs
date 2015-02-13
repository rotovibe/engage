using MongoDB.Bson;
using Phytel.API.DataDomain.Step.DTO;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.Step
{
	public class TextStepMongoContext : MongoContext
	{
		private static string COLL_Step = "Step";

		public TextStepMongoContext(string contractDBName)
			: base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
			TextSteps = new MongoSet<METext, ObjectId>(this, COLL_Step);
		}

		public MongoSet<METext, ObjectId> TextSteps { get; private set; }
	}
}
