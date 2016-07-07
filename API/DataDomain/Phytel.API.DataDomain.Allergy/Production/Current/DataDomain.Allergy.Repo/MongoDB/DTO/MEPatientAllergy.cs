using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEPatientAllergy : IMongoEntity<ObjectId>, IMEEntity,IMEDataSource
    {
        public MEPatientAllergy(string userId)
        {
            Id = ObjectId.GenerateNewId();
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = System.DateTime.UtcNow;
            Version = 1.0;
        }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; private set; }

        public const string AllergyIdProperty = "aid";
        [BsonElement(AllergyIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId AllergyId { get; set; }

        public const string PatientIdProperty = "pid";
        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId PatientId { get; set; }

        public const string SeverityIdProperty = "sevid";
        [BsonElement(SeverityIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? SeverityId { get; set; }

        public const string ReactionIdsProperty = "rctid";
        [BsonElement(ReactionIdsProperty)]
        [BsonIgnoreIfNull(true)]        
        public List<ObjectId> ReactionIds { get; set; }
       
        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status StatusId { get; set; }

        public const string SourceIdProperty = "srcid";
        [BsonElement(SourceIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId SourceId { get; set; }

        public const string StartDateProperty = "sd";
        [BsonElement(StartDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? StartDate { get; set; }

        public const string EndDateProperty = "ed";
        [BsonElement(EndDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? EndDate { get; set; }

        public const string NotesProperty = "nts";
        [BsonElement(NotesProperty)]
        [BsonIgnoreIfNull(true)]
        public string Notes { get; set; }

        public const string SystemProperty = "sys";
        [BsonElement(SystemProperty)]
        [BsonIgnoreIfNull(true)]
        public string SystemName { get; set; }

        public const string DataSourceProperty = "dsrc";
        [BsonElement(DataSourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string DataSource { get; set; }

        public const string ExternalRecordIdProperty = "extrid";
        [BsonElement(ExternalRecordIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ExternalRecordId { get; set; }

        public const string CodingSystemProperty = "csid";
        [BsonElement(CodingSystemProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? CodingSystemId { get; set; }

        public const string CodeProperty = "csc";
        [BsonElement(CodeProperty)]
        [BsonIgnoreIfNull(true)]
        public string Code { get; set; }

        #region Base elements
        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
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

        public const string ExtraElementsProperty = "ex";
        [BsonElement(ExtraElementsProperty)]
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; } 
        #endregion
    }
}
