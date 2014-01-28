using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEContact : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEContact() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string PatientIdProperty = "pid";
        public const string UserIdProperty = "uid";
        public const string ModesProperty = "ms";
        public const string PhonesProperty = "phs";
        public const string EmailsProperty = "ems";
        public const string AddressessProperty = "adds";
        public const string WeekDaysProperty = "wds";
        public const string TimesOfDaysProperty = "tofds";
        public const string TimeZoneProperty = "tz";
        public const string LanguagesProperty = "lans";

        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? PatientId { get; set; }

        [BsonElement(UserIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? UserId { get; set; }

        [BsonElement(ModesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MECommMode> Modes { get; set; }

        [BsonElement(PhonesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MEPhone> Phones { get; set; }

        [BsonElement(EmailsProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MEEmail> Emails { get; set; }

        [BsonElement(AddressessProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MEAddress> Addresses { get; set; }

        [BsonElement(WeekDaysProperty)]
        [BsonIgnoreIfNull(true)]
        public List<int> WeekDays { get; set; }

        [BsonElement(TimesOfDaysProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> TimesOfDays { get; set; }

        [BsonElement(TimeZoneProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? TimeZone { get; set; }

        [BsonElement(LanguagesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MELanguage> Languages { get; set; }
        
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
}
