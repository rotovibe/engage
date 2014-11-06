using MongoDB.Bson;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.Mongo.Linq;

namespace DataDomain.Medication.Repo
{
	public class MedicationMongoContext : MongoContext
	{
		private static string COLL_MedicationS = "Medication";
        private static string COLL_PatientMedSupp = "PatientMedSupp";
		public string ContractName { get; set; }

		public MedicationMongoContext(string contractDBName)
			: base(contractDBName, true)
		{
			ContractName = contractDBName;
			Medications = new MongoSet<MEMedication, ObjectId>(this, COLL_MedicationS);
            PatientMedSupps = new MongoSet<MEPatientMedSupp, ObjectId>(this, COLL_PatientMedSupp);
		}

		public MongoSet<MEMedication, ObjectId> Medications { get; private set; }
        public MongoSet<MEPatientMedSupp, ObjectId> PatientMedSupps { get; private set; }
	}
}
