using MongoDB.Bson;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.PatientSystem
{
    public class PatientSystemMongoContext : MongoContext
    {
        private static string COLL_PatientSystemS = "PatientSystem";
        private static string COLL_System = "System";

        public PatientSystemMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
        {
            PatientSystems = new MongoSet<MEPatientSystem, ObjectId>(this, COLL_PatientSystemS);
            Systems = new MongoSet<MESystem, ObjectId>(this, COLL_System);
        }

        public MongoSet<MEPatientSystem, ObjectId> PatientSystems { get; private set; }
        public MongoSet<MESystem, ObjectId> Systems { get; private set; }
    }
}
