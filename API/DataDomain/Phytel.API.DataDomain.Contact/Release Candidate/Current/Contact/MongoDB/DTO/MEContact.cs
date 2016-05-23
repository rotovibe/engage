using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { PatientIdProperty, DeleteFlagProperty }, Unique = false)]
    [MongoIndex(Keys = new string[] { ResourceIdProperty, PatientIdProperty, DeleteFlagProperty }, Unique = false)]
    public class MEContact : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEContact(string userId, DateTime? createdOn)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = createdOn == null || createdOn.Equals(new DateTime()) ? DateTime.UtcNow : (DateTime)createdOn;
        }

        public const string IdProperty = "_id";
        public const string PatientIdProperty = "pid";
        public const string ResourceIdProperty = "rid";
        public const string FirstNameProperty = "fn";
        public const string MiddleNameProperty = "mn";
        public const string LastNameProperty = "ln";
        public const string PreferredNameProperty = "pfn";
        public const string GenderProperty = "g";
        public const string ModesProperty = "mds";
        public const string PhonesProperty = "phs";
        public const string EmailsProperty = "ems";
        public const string AddressessProperty = "adds";
        public const string WeekDaysProperty = "wds";
        public const string TimesOfDaysProperty = "tofds";
        public const string TimeZoneProperty = "tz";
        public const string LanguagesProperty = "lans";

        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";
        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedOnProperty = "rcon";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? PatientId { get; set; }

        [BsonElement(ResourceIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ResourceId { get; set; }

        [BsonElement(FirstNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string FirstName { get; set; }

        [BsonElement(LastNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string LastName { get; set; }

        [BsonElement(MiddleNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string MiddleName { get; set; }

        [BsonElement(PreferredNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string PreferredName { get; set; }

        [BsonElement(GenderProperty)]
        [BsonIgnoreIfNull(true)]
        public string Gender { get; set; }

        [BsonElement(ModesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<CommMode> Modes { get; set; }

        [BsonElement(PhonesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<Phone> Phones { get; set; }

        [BsonElement(EmailsProperty)]
        [BsonIgnoreIfNull(true)]
        public List<Email> Emails { get; set; }

        [BsonElement(AddressessProperty)]
        [BsonIgnoreIfNull(true)]
        public List<Address> Addresses { get; set; }

        [BsonElement(WeekDaysProperty)]
        [BsonIgnoreIfNull(true)]
        public List<int> WeekDays { get; set; }

        [BsonElement(TimesOfDaysProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> TimesOfDays { get; set; }

        [BsonElement(TimeZoneProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? TimeZoneId { get; set; }

        [BsonElement(LanguagesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<Language> Languages { get; set; }

        public const string RecentListProperty = "rctlst";
        [BsonElement(RecentListProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> RecentList { get; set; }

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        [BsonElement(UpdatedByProperty)]
        public ObjectId? UpdatedBy { get; set; }

        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime? TTLDate { get; set; }

        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime? LastUpdatedOn { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; private set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; private set; }
    }
}
