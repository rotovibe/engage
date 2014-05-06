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
        private static string COLL_ProgramS = "Program";
        private static string COLL_StepResponseS = "Response";
        private static string COLL_ProgramAttributeS = "PatientProgramAttribute";
        private static string COLL_ModuleS = "Module";
        private static string COLL_ActionS = "Action";
        private static string COLL_Step = "Step";

        public ProgramDesignMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
            ProgramDesigns = new MongoSet<MEProgramDesign, ObjectId>(this, COLL_ProgramDesignS);
            Programs = new MongoSet<MEProgram, ObjectId>(this, COLL_ProgramS);
            //Responses = new MongoSet<MEResponse, ObjectId>(this, COLL_StepResponseS);
            //ProgramAttributes = new MongoSet<MEProgramAttribute, ObjectId>(this, COLL_ProgramAttributeS);
            Modules = new MongoSet<MEModule, ObjectId>(this, COLL_ModuleS);
            Actions = new MongoSet<MEAction, ObjectId>(this, COLL_ActionS);
            TextSteps = new MongoSet<METext, ObjectId>(this, COLL_Step);
            YesNoSteps = new MongoSet<MEYesNo, ObjectId>(this, COLL_Step);
            

		}

        public MongoSet<MEProgramDesign, ObjectId> ProgramDesigns { get; private set; }
        public MongoSet<MEProgram, ObjectId> Programs { get; private set; }
        public MongoSet<MEResponse, ObjectId> Responses { get; private set; }
        public MongoSet<MEProgramAttribute, ObjectId> ProgramAttributes { get; private set; }
        public MongoSet<MEModule, ObjectId> Modules { get; private set; }
        public MongoSet<MEAction, ObjectId> Actions { get; private set; }
        public MongoSet<METext, ObjectId> TextSteps { get; private set; }
        public MongoSet<MEYesNo, ObjectId> YesNoSteps { get; private set; }
    }
}
