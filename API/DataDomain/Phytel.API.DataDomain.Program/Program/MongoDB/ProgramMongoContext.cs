using MongoDB.Bson;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.Program
{
    public class ProgramMongoContext : MongoContext
    {
        private static string COLL_ContractProgramS = "Program"; // switch to Program when done 
        private static string COLL_PatientProgramS = "PatientProgram";
        private static string COLL_PatientProgramResponseS = "PatientProgramResponse";
        private static string COLL_StepResponseS = "Response";
        private static string COLL_ProgramAttributeS = "PatientProgramAttribute";

        public ProgramMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
        {
            Programs = new MongoSet<MEProgram, ObjectId>(this, COLL_ContractProgramS);
            PatientPrograms = new MongoSet<MEPatientProgram, ObjectId>(this, COLL_PatientProgramS);
            PatientProgramResponses = new MongoSet<MEPatientProgramResponse, ObjectId>(this, COLL_PatientProgramResponseS);
            Responses = new MongoSet<MEResponse, ObjectId>(this, COLL_StepResponseS);
            ProgramAttributes = new MongoSet<MEProgramAttribute, ObjectId>(this, COLL_ProgramAttributeS);
        }

        public MongoSet<MEProgram, ObjectId> Programs { get; private set; }
        public MongoSet<MEPatientProgram, ObjectId> PatientPrograms { get; private set; }
        public MongoSet<MEPatientProgramResponse, ObjectId> PatientProgramResponses { get; private set; }
        public MongoSet<MEResponse, ObjectId> Responses { get; private set; }
        public MongoSet<MEProgramAttribute, ObjectId> ProgramAttributes { get; private set; }
    }
}
