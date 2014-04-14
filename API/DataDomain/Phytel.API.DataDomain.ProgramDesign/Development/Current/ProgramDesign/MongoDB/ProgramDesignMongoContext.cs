using MongoDB.Bson;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO;
using Phytel.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public class ProgramDesignMongoContext : MongoContext
    {
        private static string COLL_ProgramDesignS = "ProgramDesign";
        private static string COLL_ContractProgramS = "Program"; // switch to Program when done 
        private static string COLL_PatientProgramS = "PatientProgram";
        private static string COLL_PatientProgramResponseS = "PatientProgramResponse";
        private static string COLL_StepResponseS = "Response";
        private static string COLL_ProgramAttributeS = "PatientProgramAttribute";

        public ProgramDesignMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
            ProgramDesigns = new MongoSet<MEProgramDesign, ObjectId>(this, COLL_ProgramDesignS);
            Programs = new MongoSet<MEProgram, ObjectId>(this, COLL_ContractProgramS);
            Responses = new MongoSet<MEResponse, ObjectId>(this, COLL_StepResponseS);
            ProgramAttributes = new MongoSet<MEProgramAttribute, ObjectId>(this, COLL_ProgramAttributeS);
		}

        public MongoSet<MEProgramDesign, ObjectId> ProgramDesigns { get; private set; }
        public MongoSet<MEProgram, ObjectId> Programs { get; private set; }
        public MongoSet<MEResponse, ObjectId> Responses { get; private set; }
        public MongoSet<MEProgramAttribute, ObjectId> ProgramAttributes { get; private set; }
    }
}
