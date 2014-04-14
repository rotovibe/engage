using MongoDB.Bson;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public class ProgramDesignMongoContext : MongoContext
    {
        private static string COLL_ProgramDesignS = "ProgramDesign";

        public ProgramDesignMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            ProgramDesigns = new MongoSet<MEProgramDesign, ObjectId>(this, COLL_ProgramDesignS);
		}

        public MongoSet<MEProgramDesign, ObjectId> ProgramDesigns { get; private set; }
    }
}
