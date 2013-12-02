using MongoDB.Bson;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.Program
{
    public class ProgramMongoContext : MongoContext
    {
        private static string COLL_ProgramS = "Program";

        public ProgramMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            Programs = new MongoSet<MEProgram, ObjectId>(this, COLL_ProgramS);
		}

        public MongoSet<MEProgram, ObjectId> Programs { get; private set; }
    }
}
