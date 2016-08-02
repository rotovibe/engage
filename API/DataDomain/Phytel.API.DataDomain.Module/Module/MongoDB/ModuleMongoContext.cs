using MongoDB.Bson;
using Phytel.API.DataDomain.Module.DTO;
using Phytel.API.DataDomain.Module.MongoDB.DTO;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.Module
{
	public class ModuleMongoContext : MongoContext
	{
		private static string COLL_ModuleS = "Module";

		public ModuleMongoContext(string contractDBName)
			: base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
			Modules = new MongoSet<MEModule, ObjectId>(this, COLL_ModuleS);
		}

		public MongoSet<MEModule, ObjectId> Modules { get; private set; }
	}
}
