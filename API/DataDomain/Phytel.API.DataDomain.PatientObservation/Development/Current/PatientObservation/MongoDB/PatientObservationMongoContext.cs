using MongoDB.Bson;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.PatientObservation
{
    public class PatientObservationMongoContext : MongoContext
    {
        private static string COLL_PatientObservationS = "PatientObservation";

        public PatientObservationMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            PatientObservations = new MongoSet<MEPatientObservation, ObjectId>(this, COLL_PatientObservationS);
		}

        public MongoSet<MEPatientObservation, ObjectId> PatientObservations { get; private set; }
    }
}
