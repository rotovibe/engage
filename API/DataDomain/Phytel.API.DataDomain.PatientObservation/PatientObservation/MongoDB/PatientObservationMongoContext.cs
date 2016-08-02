using MongoDB.Bson;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.PatientObservation
{
	public class PatientObservationMongoContext : MongoContext
	{
		private static string COLL_PatientObservationS = "PatientObservation";
		private static string COLL_ObservationS = "Observation";

		public PatientObservationMongoContext(string contractDBName)
			: base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
			PatientObservations = new MongoSet<MEPatientObservation, ObjectId>(this, COLL_PatientObservationS);
			Observations = new MongoSet<MEObservation, ObjectId>(this, COLL_ObservationS);
		}

		public MongoSet<MEPatientObservation, ObjectId> PatientObservations { get; private set; }
		public MongoSet<MEObservation, ObjectId> Observations { get; private set; }
	}
}
