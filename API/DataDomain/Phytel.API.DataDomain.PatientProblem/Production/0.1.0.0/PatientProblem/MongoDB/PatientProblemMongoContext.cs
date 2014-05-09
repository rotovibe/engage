using MongoDB.Bson;
using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.PatientProblem
{
    public class PatientProblemMongoContext : MongoContext
    {
        private static string COLL_PATIENTPROBLEMS = "PatientProblem";

        public PatientProblemMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
            PatientProblems = new MongoSet<MEPatientProblem, ObjectId>(this, COLL_PATIENTPROBLEMS);
        }

        public MongoSet<MEPatientProblem, ObjectId> PatientProblems { get; private set; }
    }
}
