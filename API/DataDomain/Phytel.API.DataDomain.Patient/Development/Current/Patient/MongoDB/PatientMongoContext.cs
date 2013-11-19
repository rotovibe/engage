using MongoDB.Bson;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.Patient
{
    public class PatientMongoContext : MongoContext
    {
        private static string COLL_PATIENTS = "Patient";

        public PatientMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            Patients = new MongoSet<MEPatient, ObjectId>(this, COLL_PATIENTS);
		}

        public MongoSet<MEPatient, ObjectId> Patients { get; private set; }
    }
}
