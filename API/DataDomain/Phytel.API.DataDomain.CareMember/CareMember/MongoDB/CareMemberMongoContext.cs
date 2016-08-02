using MongoDB.Bson;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.CareMember
{
	public class CareMemberMongoContext : MongoContext
	{
		private static string COLL_CareMemberS = "CareMember";

		public CareMemberMongoContext(string contractDBName)
			: base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
			CareMembers = new MongoSet<MECareMember, ObjectId>(this, COLL_CareMemberS);
		}

		public MongoSet<MECareMember, ObjectId> CareMembers { get; private set; }
	}
}
