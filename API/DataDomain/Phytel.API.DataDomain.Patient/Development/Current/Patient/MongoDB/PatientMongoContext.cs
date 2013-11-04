using MongoDB.Bson;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.Patient
{
    public class PatientMongoContext : MongoContext
    {
        private static string COLL_PATIENTS = "Patient";
        private static string COLL_PATIENTSEARCH = "PatientSearch";

        public PatientMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            Patients = new MongoSet<MEPatient, ObjectId>(this, COLL_PATIENTS);
            //PatientSearch = new MongoSet<MEAPISession, ObjectId>(this, COLL_APISESSIONS);
		}

        public MongoSet<MEPatient, ObjectId> Patients { get; private set; }
        //public MongoSet<MEAPISession, ObjectId> PatientSearch { get; private set; }
    }
}
