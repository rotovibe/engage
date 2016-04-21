using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [BsonIgnoreExtraElements]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { PatientIdProperty, DeleteFlagProperty }, Unique = false)]
    [MongoIndex(Keys = new string[] { ResourceIdProperty, PatientIdProperty, DeleteFlagProperty }, Unique = false)]
    public class MEContact : IMongoEntity<ObjectId>, IMEEntity, IMEDataSource
    {
        public MEContact(string userId, DateTime? createdOn)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = createdOn == null || createdOn.Equals(new DateTime()) ? DateTime.UtcNow : (DateTime)createdOn;
        }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string PatientIdProperty = "pid";
        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? PatientId { get; set; }

        public const string ResourceIdProperty = "rid";
        [BsonElement(ResourceIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ResourceId { get; set; }

        public const string FirstNameProperty = "fn";
        [BsonElement(FirstNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string FirstName { get; set; }

        public const string LoweredFirstNameProperty = "lfn";
        [BsonElement(LoweredFirstNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string LoweredFirstName { get; set; }

        public const string LastNameProperty = "ln";
        [BsonElement(LastNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string LastName { get; set; }

        public const string LoweredLastNameProperty = "lln";
        [BsonElement(LoweredLastNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string LoweredLastName { get; set; }

        public const string MiddleNameProperty = "mn";
        [BsonElement(MiddleNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string MiddleName { get; set; }

        public const string PreferredNameProperty = "pfn";
        [BsonElement(PreferredNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string PreferredName { get; set; }

        public const string GenderProperty = "g";
        [BsonElement(GenderProperty)]
        [BsonIgnoreIfNull(true)]
        public string Gender { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status Status { get; set; }

        public const string DeceasedProperty = "dec";
        [BsonElement(DeceasedProperty)]
        [BsonIgnoreIfNull(true)]
        public Deceased Deceased { get; set; }

        public const string PrefixProperty = "pfx";
        [BsonElement(PrefixProperty)]
        [BsonIgnoreIfNull(true)]
        public string Prefix { get; set; }

        public const string SuffixProperty = "sfx";
        [BsonElement(SuffixProperty)]
        [BsonIgnoreIfNull(true)]
        public string Suffix { get; set; }

        public const string ContactSubTypesProperty = "csubtypes";
        [BsonElement(ContactSubTypesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MEContactSubType> ContactSubTypes { get; set; }

        public const string ContactTypeIdProperty = "ctypeid";
        [BsonElement(ContactTypeIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId ContactTypeId { get; set; }

        #region Communication fields
        public const string ModesProperty = "mds";
        [BsonElement(ModesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<CommMode> Modes { get; set; }

        public const string PhonesProperty = "phs";
        [BsonElement(PhonesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<Phone> Phones { get; set; }

        public const string EmailsProperty = "ems";
        [BsonElement(EmailsProperty)]
        [BsonIgnoreIfNull(true)]
        public List<Email> Emails { get; set; }


        public const string AddressessProperty = "adds";
        [BsonElement(AddressessProperty)]
        [BsonIgnoreIfNull(true)]
        public List<Address> Addresses { get; set; }

        public const string WeekDaysProperty = "wds";
        [BsonElement(WeekDaysProperty)]
        [BsonIgnoreIfNull(true)]
        public List<int> WeekDays { get; set; }

        public const string TimesOfDaysProperty = "tofds";
        [BsonElement(TimesOfDaysProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> TimesOfDays { get; set; }

        public const string TimeZoneProperty = "tz";
        [BsonElement(TimeZoneProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? TimeZoneId { get; set; }

        public const string LanguagesProperty = "lans";
        [BsonElement(LanguagesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<Language> Languages { get; set; } 
        #endregion

        public const string RecentListProperty = "rctlst";
        [BsonElement(RecentListProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> RecentList { get; set; }

        public const string ExternalRecordIdProperty = "extrid";
        [BsonElement(ExternalRecordIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ExternalRecordId { get; set; }

        public const string DataSourceProperty = "dsrc";
        [BsonElement(DataSourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string DataSource { get; set; }

        #region Base fields
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? UpdatedBy { get; set; }

        public const string DeleteFlagProperty = "del";
        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        public const string TTLDateProperty = "ttl";
        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime? TTLDate { get; set; }

        public const string LastUpdatedOnProperty = "uon";
        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime? LastUpdatedOn { get; set; }

        public const string RecordCreatedByProperty = "rcby";
        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; private set; }

        public const string RecordCreatedOnProperty = "rcon";
        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; private set; } 
        #endregion
    }
}
