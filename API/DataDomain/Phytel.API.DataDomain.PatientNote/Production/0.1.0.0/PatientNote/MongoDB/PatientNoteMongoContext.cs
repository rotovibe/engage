using MongoDB.Bson;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.PatientNote
{
    public class PatientNoteMongoContext : MongoContext
    {
        private static string COLL_PatientNoteS = "PatientNote";

        public PatientNoteMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
            PatientNotes = new MongoSet<MEPatientNote, ObjectId>(this, COLL_PatientNoteS);
		}

        public MongoSet<MEPatientNote, ObjectId> PatientNotes { get; private set; }
    }
}
