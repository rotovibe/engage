using MongoDB.Bson;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.Patient
{
	public class PatientMongoContext : MongoContext
	{
		private static string COLL_PATIENTS = "Patient";
		private static string COLL_PATIENTUSERS = "PatientUser";
		private static string COLL_COHORTPATIENTVIEWS = "CohortPatientView";

		public PatientMongoContext(string contractDBName)
			: base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
			Patients = new MongoSet<MEPatient, ObjectId>(this, COLL_PATIENTS);
			PatientUsers = new MongoSet<MEPatientUser, ObjectId>(this, COLL_PATIENTUSERS);
			CohortPatientViews = new MongoSet<MECohortPatientView, ObjectId>(this, COLL_COHORTPATIENTVIEWS);
		}

		public MongoSet<MEPatient, ObjectId> Patients { get; private set; }
		public MongoSet<MEPatientUser, ObjectId> PatientUsers { get; private set; }
		public MongoSet<MECohortPatientView, ObjectId> CohortPatientViews { get; private set; }
	}
}
