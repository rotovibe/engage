using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive=0)]
    public class MEPatient : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatient(string userId, DateTime? createdOn)
        {
            Id = ObjectId.GenerateNewId();  
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = createdOn == null || createdOn .Equals(new DateTime())? DateTime.UtcNow : (DateTime)createdOn;
        }

        public const string IdProperty = "_id";
        public const string DisplayPatientSystemIdProperty = "dpsid";
        public const string FirstNameProperty = "fn";
        public const string LastNameProperty = "ln";
        public const string GenderProperty = "g";
        public const string DOBProperty = "dob";
        public const string MiddleNameProperty = "mn";
        public const string SuffixProperty = "sfx";
        public const string PreferredNameProperty = "pfn";
        public const string PriorityProperty = "pri";
        public const string BackgroundProperty = "bkgrd";
        public const string ClinicalBackgroundProperty = "cbkgrd";
        public const string LastFourSSNProperty = "lssn";
        public const string FullSSNProperty = "fssn";
        public const string StatusProperty = "sts";
        public const string ReasonProperty = "rsn";
        public const string ProtectedProperty = "prot";
        public const string MaritalStatusProperty = "msts";
        public const string DeceasedProperty = "dec";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";
        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedOnProperty = "rcon";
        public const string PrefixProperty = "pfx";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(DisplayPatientSystemIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? DisplayPatientSystemId { get; set; }

        [BsonElement(FirstNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string FirstName { get; set; }

        [BsonElement(LastNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string LastName { get; set; }

        [BsonElement(SuffixProperty)]
        [BsonIgnoreIfNull(true)]
        public string Suffix { get; set; }

        [BsonElement(PreferredNameProperty)]
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
        public PriorityData Priority { get; set; }

        [BsonElement(BackgroundProperty)]
        [BsonIgnoreIfNull(true)]
        public string Background { get; set; }

        [BsonElement(ClinicalBackgroundProperty)]
        [BsonIgnoreIfNull(true)]
        public string ClinicalBackground { get; set; }

        [BsonElement(LastFourSSNProperty)]
        [BsonIgnoreIfNull(true)]
        public string LastFourSSN { get; set; }

        [BsonElement(FullSSNProperty)]
        [BsonIgnoreIfNull(true)]
        public string FullSSN { get; set; }

        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status Status { get; set; }

        [BsonElement(ReasonProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? ReasonId { get; set; }

        public const string DataSourceProperty = "dsrc";
        [BsonElement(DataSourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string DataSource { get; set; }

        public const string StatusDataSourceProperty = "stsdsrc";
        [BsonElement(StatusDataSourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string StatusDataSource { get; set; }

        [BsonElement(MaritalStatusProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? MaritalStatusId { get; set; }

        [BsonElement(ProtectedProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Protected { get; set; }

        [BsonElement(DeceasedProperty)]
        [BsonIgnoreIfNull(true)]
        public Deceased Deceased { get; set; }

        public const string ExternalRecordIdProperty = "extrid";
        [BsonElement(ExternalRecordIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ExternalRecordId { get; set; }

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
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public System.DateTime? TTLDate { get; set; }

        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public System.DateTime? LastUpdatedOn { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; private set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; private set; }

        [BsonElement(PrefixProperty)]
        [BsonIgnoreIfNull(true)]
        public string Prefix { get; set; }
    }

    public enum Priority
    {
        NotSet = 0,
        Low = 1,
        Medium = 2,
        High = 3
    }
}
