using MongoDB.Bson;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.Mongo.Linq;

namespace DataDomain.Allergy.Repo
{
	public class AllergyMongoContext : MongoContext
	{
		private static string COLL_Allergy = "DdAllergy";
		private static string COLL_PatientAllergy = "PatientAllergy";
		public string ContractName { get; set; }

		public AllergyMongoContext(string contractDBName)
			: base(contractDBName, true)
		{
			ContractName = contractDBName;
			Allergy = new MongoSet<MEAllergy, ObjectId>(this, COLL_Allergy);
			PatientAllergy = new MongoSet<MEPatientAllergy, ObjectId>(this, COLL_PatientAllergy);
		}

		public MongoSet<MEAllergy, ObjectId> Allergy { get; private set; }
		public MongoSet<MEPatientAllergy, ObjectId> PatientAllergy { get; private set; }
	}
}
