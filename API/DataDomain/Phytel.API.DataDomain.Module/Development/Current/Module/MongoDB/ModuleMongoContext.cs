using MongoDB.Bson;
using Phytel.API.DataDomain.Module.DTO;
using Phytel.API.DataDomain.Module.MongoDB.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.Module
{
    public class ModuleMongoContext : MongoContext
    {
        private static string COLL_ModuleS = "Module";

        public ModuleMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            Modules = new MongoSet<MEModule, ObjectId>(this, COLL_ModuleS);
		}

        public MongoSet<MEModule, ObjectId> Modules { get; private set; }
    }
}
