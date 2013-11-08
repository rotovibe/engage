using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    [BsonIgnoreExtraElements(true)]
    [MongoIndex(Keys = new string[] { PatientIDProperty })]
    public class MEPatientProblem : IMongoEntity<ObjectId>
    {
        public MEPatientProblem() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string PatientIDProperty = "pid";
        public const string DisplayNameProperty = "dn";
        public const string CodeProperty = "cd";
        public const string StatusProperty = "stat";
        public const string CategoryProperty = "cat";
        public const string DisplayProperty = "dis";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(PatientIDProperty)]
        [BsonIgnoreIfNull(true)]
        public string PatientID { get; set; }

        [BsonElement(DisplayNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string DisplayName { get; set; }

        [BsonElement(CodeProperty)]
        [BsonIgnoreIfNull(true)]
        public string Code { get; set; }

        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status Status { get; set; }

        [BsonElement(CategoryProperty)]
        [BsonIgnoreIfNull(true)]
        public Category Category { get; set; }

        [BsonElement(DisplayProperty)]
        [BsonIgnoreIfNull(true)]
        public bool DisplayCondition { get; set; }
    }

    public enum Status
    { 
        Active,
        Inactive
    }

    public enum Category
    {
        Chronic
    }
}

    