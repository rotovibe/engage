using MongoDB.Bson;
using Phytel.API.DataDomain.Search.DTO;
using Phytel.Services.Mongo.Linq;

namespace DataDomain.Search.Repo
{
	public class SearchMongoContext : MongoContext
	{
		private static string COLL_SearchS = "Search";
		public string ContractName { get; set; }

		public SearchMongoContext(string contractDBName)
			: base(contractDBName, true)
		{
			ContractName = contractDBName;
			Searchs = new MongoSet<MESearch, ObjectId>(this, COLL_SearchS);
		}

		public MongoSet<MESearch, ObjectId> Searchs { get; private set; }
	}
}
