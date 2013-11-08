using MongoDB.Bson;
using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.PatientProblem
{
    public class PatientProblemMongoContext : MongoContext
    {
        private static string COLL_PATIENTPROBLEMS = "PatientProblem";

        public PatientProblemMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            PatientProblems = new MongoSet<MEPatientProblem, ObjectId>(this, COLL_PATIENTPROBLEMS);
        }

        public MongoSet<MEPatientProblem, ObjectId> PatientProblems { get; private set; }
    }
}
