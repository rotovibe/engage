using MongoDB.Bson;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.PatientSystem
{
    public class PatientSystemMongoContext : MongoContext
    {
        private static string COLL_PatientSystemS = "PatientSystem";

        public PatientSystemMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
            PatientSystems = new MongoSet<MEPatientSystem, ObjectId>(this, COLL_PatientSystemS);
		}

        public MongoSet<MEPatientSystem, ObjectId> PatientSystems { get; private set; }
    }
}
