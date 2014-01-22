using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive=0)]
    public class MEPatient : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatient() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string DisplayPatientSystemIDProperty = "dpsid";
        public const string FirstNameProperty = "fn";
        public const string LastNameProperty = "ln";
        public const string GenderProperty = "gn";
        public const string DOBProperty = "dob";
        public const string MiddleNameProperty = "mn";
        public const string SuffixProperty = "sfx";
        public const string PreferredProperty = "pfn";
        public const string PriorityProperty = "pri";
        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(DisplayPatientSystemIDProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? DisplayPatientSystemID { get; set; }

        [BsonElement(FirstNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string FirstName { get; set; }

        [BsonElement(LastNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string LastName { get; set; }

        [BsonElement(SuffixProperty)]
        [BsonIgnoreIfNull(true)]
        public string Suffix { get; set; }

        [BsonElement(PreferredProperty)]
        [BsonIgnoreIfNull(true)]
        public string PreferredName { get; set; }

        [BsonElement(MiddleNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string MiddleName { get; set; }

        [BsonElement(GenderProperty)]
        [BsonIgnoreIfNull(true)]
        public string Gender { get; set; }

        [BsonElement(DOBProperty)]
        [BsonIgnoreIfNull(true)]
        public string DOB { get; set; }

        [BsonElement(PriorityProperty)]
        [BsonIgnoreIfNull(true)]
        public MEPriority Priority { get; set; }

        [BsonElement(ExtraElementsProperty)]
        [BsonExtraElements()]
        [BsonIgnoreIfNull(true)]
        public Dictionary<string, object> ExtraElements { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue("v1")]
        public string Version { get; set; }

        [BsonElement(UpdatedByProperty)]
        [BsonDefaultValue("-100")]
        public string UpdatedBy { get; set; }

        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public System.DateTime? TTLDate { get; set; }

        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public System.DateTime? LastUpdatedOn { get; set; }
    }

    public enum MEPriority
    {
        NotSet = 0,
        Low = 1,
        Medium = 2,
        High = 3
    }
}
