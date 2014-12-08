using MongoDB.Bson;
using Phytel.API.DataDomain.Contract.DTO;
using Phytel.Services.Mongo;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.Contract
{
    public class ContractMongoContext : MongoContext
    {
        private static string COLL_ContractS = "Contract";

        public ContractMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
            Contracts = new MongoSet<MEContract, ObjectId>(this, COLL_ContractS);
		}

        public MongoSet<MEContract, ObjectId> Contracts { get; private set; }
    }
}
