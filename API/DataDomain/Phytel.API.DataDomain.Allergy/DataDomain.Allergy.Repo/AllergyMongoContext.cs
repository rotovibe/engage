using MongoDB.Bson;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.Services.Mongo.Linq;

namespace DataDomain.Allergy.Repo
{
	public class AllergyMongoContext : MongoContext
	{
		private static string COLL_Allergy = "Allergy";
		private static string COLL_PatientAllergy = "PatientAllergy";
		public string ContractName { get; set; }

		public AllergyMongoContext(string contractDBName)
			: base(contractDBName, true)
		{
			ContractName = contractDBName;
			Allergies = new MongoSet<MEAllergy, ObjectId>(this, COLL_Allergy);
			PatientAllergies = new MongoSet<MEPatientAllergy, ObjectId>(this, COLL_PatientAllergy);
		}

		public MongoSet<MEAllergy, ObjectId> Allergies { get; private set; }
		public MongoSet<MEPatientAllergy, ObjectId> PatientAllergies { get; private set; }
	}
}
