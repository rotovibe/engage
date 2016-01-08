using System.Configuration;
using MongoDB.Bson;
using Phytel.Services.Mongo.Linq;

namespace Phytel.API.DataDomain.PatientNote.Repo
{
	public class PatientNoteMongoContext : MongoContext
	{
		private static string COLL_PatientNoteS = "PatientNote";
		private static string COLL_PatientUtilizationS = "PatientUtilization";
		public string ContractName { get; set; }

		public PatientNoteMongoContext(string contractDBName)
			: base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
			PatientNotes = new MongoSet<MEPatientNote, ObjectId>(this, COLL_PatientNoteS);
			PatientUtilizations = new MongoSet<MEPatientUtilization, ObjectId>(this, COLL_PatientUtilizationS);
		}

		public MongoSet<MEPatientNote, ObjectId> PatientNotes { get; private set; }
		public MongoSet<MEPatientUtilization, ObjectId> PatientUtilizations { get; private set; }
	}
}
