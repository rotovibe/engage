using MongoDB.Bson;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.LookUp
{
	public class LookUpMongoContext : MongoContext
	{
		private static string COLL_LOOKUP = "LookUp";
        private static string COLL_SETTING = "Setting";

		public LookUpMongoContext(string contractDBName)
			: base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
			LookUps = new MongoSet<MELookup, ObjectId>(this, COLL_LOOKUP);
            Settings = new MongoSet<MESetting, ObjectId>(this, COLL_SETTING);
		}

		public MongoSet<MELookup, ObjectId> LookUps { get; private set; }
        public MongoSet<MESetting, ObjectId> Settings { get; private set; }
	}
}
