using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [BsonIgnoreExtraElements(true)]
    [MongoIndex(Keys = new string[] { PatientIDProperty })]
    public class MEPatient : IMongoEntity<ObjectId>
    {
        public MEPatient() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string PatientIDProperty = "pid";
        public const string FirstNameProperty = "fn";
        public const string LastNameProperty = "ln";
        public const string GenderProperty = "gn";
        public const string DOBProperty = "dob";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(PatientIDProperty)]
        [BsonIgnoreIfNull(true)]
        public string PatientID { get; set; }

        [BsonElement(FirstNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string FirstName { get; set; }

        [BsonElement(LastNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string LastName { get; set; }

        [BsonElement(GenderProperty)]
        [BsonIgnoreIfNull(true)]
        public string Gender { get; set; }

        [BsonElement(DOBProperty)]
        [BsonIgnoreIfNull(true)]
        public string DOB { get; set; }
    }
}
