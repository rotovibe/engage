using MongoDB.Bson;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.Program
{
    public class ProgramMongoContext : MongoContext
    {
        private static string COLL_ContractProgramS = "ContractProgram"; // switch to Program when done 
        private static string COLL_PatientProgramS = "PatientProgram";
        private static string COLL_PatientProgramResponseS = "PatientProgramResponse";
        private static string COLL_StepResponseS = "Response";
        private static string COLL_ProgramAttributeS = "PatientProgramAttribute";
        // this is temp!!!
        private static string COLL_TempResponseS = "Temp_Response";

        public ProgramMongoContext(string contractDBName)
            : base(contractDBName, true)
        {
            Programs = new MongoSet<MEContractProgram, ObjectId>(this, COLL_ContractProgramS);
            PatientPrograms = new MongoSet<MEPatientProgram, ObjectId>(this, COLL_PatientProgramS);
            PatientProgramResponses = new MongoSet<MEPatientProgramResponse, ObjectId>(this, COLL_PatientProgramResponseS);
            Responses = new MongoSet<MEResponse, ObjectId>(this, COLL_StepResponseS);
            ProgramAttributes = new MongoSet<MEProgramAttribute, ObjectId>(this, COLL_ProgramAttributeS);

            // temp!!
            TempProgramResponses = new MongoSet<METempResponse, ObjectId>(this, COLL_TempResponseS);
        }

        public MongoSet<MEContractProgram, ObjectId> Programs { get; private set; }
        public MongoSet<MEPatientProgram, ObjectId> PatientPrograms { get; private set; }
        public MongoSet<MEPatientProgramResponse, ObjectId> PatientProgramResponses { get; private set; }
        public MongoSet<MEResponse, ObjectId> Responses { get; private set; }
        public MongoSet<MEProgramAttribute, ObjectId> ProgramAttributes { get; private set; }

        // temp!
        public MongoSet<METempResponse, ObjectId> TempProgramResponses { get; private set; }
    }
}
