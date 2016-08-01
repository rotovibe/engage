using MongoDB.Bson;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.PatientGoal
{
    public class PatientGoalMongoContext : MongoContext
    {
        private static string COLL_PatientGoalS = "PatientGoal";
        private static string COLL_GoalS = "Goal";
        private static string COLL_PatientGoalBarrierS = "PatientBarrier";
        private static string COLL_PatientGoalTaskS = "PatientTask";
        private static string COLL_GoalTaskS = "Task";
        private static string COLL_PatientGoalInterventionS = "PatientIntervention";
        private static string COLL_GoalInterventionS = "Intervention";
        private static string COLL_Attributes = "GoalAttribute";

        public PatientGoalMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
            PatientGoals = new MongoSet<MEPatientGoal, ObjectId>(this, COLL_PatientGoalS);
            Goals = new MongoSet<MEGoal, ObjectId>(this, COLL_GoalS);
            PatientBarriers = new MongoSet<MEPatientBarrier, ObjectId>(this, COLL_PatientGoalBarrierS);
            PatientTasks = new MongoSet<MEPatientTask, ObjectId>(this, COLL_PatientGoalTaskS);
            Tasks = new MongoSet<METask, ObjectId>(this, COLL_GoalTaskS);
            PatientInterventions = new MongoSet<MEPatientIntervention, ObjectId>(this, COLL_PatientGoalInterventionS);
            Interventions = new MongoSet<MEIntervention, ObjectId>(this, COLL_GoalInterventionS);
            AttributesLibrary = new MongoSet<MEAttributeLibrary, ObjectId>(this, COLL_Attributes);
		}

        public MongoSet<MEPatientGoal, ObjectId> PatientGoals { get; private set; }
        public MongoSet<MEGoal, ObjectId> Goals { get; private set; }
        public MongoSet<MEPatientBarrier, ObjectId> PatientBarriers { get; private set; }
        public MongoSet<MEPatientTask, ObjectId> PatientTasks { get; private set; }
        public MongoSet<METask, ObjectId> Tasks { get; private set; }
        public MongoSet<MEPatientIntervention, ObjectId> PatientInterventions { get; private set; }
        public MongoSet<MEIntervention, ObjectId> Interventions { get; private set; }
        public MongoSet<MEAttributeLibrary, ObjectId> AttributesLibrary { get; private set; }
    }
}
