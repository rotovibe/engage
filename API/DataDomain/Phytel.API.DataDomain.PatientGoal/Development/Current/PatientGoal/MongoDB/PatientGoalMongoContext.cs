using MongoDB.Bson;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.PatientGoal
{
    public class PatientGoalMongoContext : MongoContext
    {
        private static string COLL_PatientGoalS = "PatientGoal";

        public PatientGoalMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            PatientGoals = new MongoSet<MEPatientGoal, ObjectId>(this, COLL_PatientGoalS);
		}

        public MongoSet<MEPatientGoal, ObjectId> PatientGoals { get; private set; }
    }
}
