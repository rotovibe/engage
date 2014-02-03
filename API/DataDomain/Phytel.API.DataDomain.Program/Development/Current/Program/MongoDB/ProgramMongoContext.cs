using MongoDB.Bson;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.Program
{
    public class ProgramMongoContext : MongoContext
    {
        private static string COLL_ProgramS = "Program";
        private static string COLL_ContractProgramS = "ContractProgram";
        private static string COLL_PatientProgramS = "PatientProgram";
        private static string COLL_StepResponseS = "Response";
        private static string COLL_ProgramAttributeS = "PatientProgramAttribute";

        public ProgramMongoContext(string contractDBName)
            : base(contractDBName, true)
        {
            Programs = new MongoSet<MEProgram, ObjectId>(this, COLL_ProgramS);
            ContractPrograms = new MongoSet<MEContractProgram, ObjectId>(this, COLL_ContractProgramS);
            PatientPrograms = new MongoSet<MEPatientProgram, ObjectId>(this, COLL_PatientProgramS);
            Responses = new MongoSet<MEResponse, ObjectId>(this, COLL_StepResponseS);
            ProgramAttributes = new MongoSet<MEProgramAttribute, ObjectId>(this, COLL_ProgramAttributeS);
        }

        public MongoSet<MEProgram, ObjectId> Programs { get; private set; }
        public MongoSet<MEContractProgram, ObjectId> ContractPrograms { get; private set; }
        public MongoSet<MEPatientProgram, ObjectId> PatientPrograms { get; private set; }
        public MongoSet<MEResponse, ObjectId> Responses { get; private set; }
        public MongoSet<MEProgramAttribute, ObjectId> ProgramAttributes { get; private set; }
    }
}
