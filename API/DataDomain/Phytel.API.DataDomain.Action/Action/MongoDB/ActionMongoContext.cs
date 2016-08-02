using MongoDB.Bson;
using Phytel.API.DataDomain.Action.DTO;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.Action
{
	public class ActionMongoContext : MongoContext
	{
		private static string COLL_ActionS = "Action";

		public ActionMongoContext(string contractDBName)
			: base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
			Actions = new MongoSet<MEAction, ObjectId>(this, COLL_ActionS);
		}

		public MongoSet<MEAction, ObjectId> Actions { get; private set; }
	}
}