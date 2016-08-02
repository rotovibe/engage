using MongoDB.Bson;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.Services.Mongo.Linq;

namespace DataDomain.Medication.Repo
{
	public class MedicationMongoContext : MongoContext
	{
		private static string COLL_MedicationS = "Medication";
        private static string COLL_PatientMedSupp = "PatientMedSupp";
        private static string COLL_MedMap = "MedicationMap";
        private static string COLL_PatientMedFrequency = "PatientMedFrequency";
		public string ContractName { get; set; }

		public MedicationMongoContext(string contractDBName)
			: base(contractDBName, true)
		{
			ContractName = contractDBName;
			Medications = new MongoSet<MEMedication, ObjectId>(this, COLL_MedicationS);
            PatientMedSupps = new MongoSet<MEPatientMedSupp, ObjectId>(this, COLL_PatientMedSupp);
            MedicationMaps = new MongoSet<MEMedicationMapping, ObjectId>(this, COLL_MedMap);
            PatientMedFrequencies = new MongoSet<MEPatientMedFrequency, ObjectId>(this, COLL_PatientMedFrequency);
		}

		public MongoSet<MEMedication, ObjectId> Medications { get; private set; }
        public MongoSet<MEMedicationMapping, ObjectId> MedicationMaps { get; private set; }
        public MongoSet<MEPatientMedSupp, ObjectId> PatientMedSupps { get; private set; }
        public MongoSet<MEPatientMedFrequency, ObjectId> PatientMedFrequencies { get; private set; }
	}
}
