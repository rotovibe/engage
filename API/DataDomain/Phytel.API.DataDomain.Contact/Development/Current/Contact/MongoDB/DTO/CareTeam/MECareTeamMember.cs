using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;

namespace Phytel.API.DataDomain.Contact.MongoDB.DTO
{

    public class MECareTeamMember : IMEDataSource
    {

        public const string IdProperty = "_id";
        public const string ContactIdProperty = "cid";
        public const string RoleIdProperty = "rid";
        public const string CustomRoleNameProperty = "crnm";
        public const string StartDateProperty = "sd";
        public const string EndDateProperty = "ed";
        public const string CoreProperty = "cor";
        public const string NotesProperty = "nts";
        public const string StatusProperty = "sts";
        public const string FrequencyProperty = "freqid";
        public const string DistanceProperty = "dist";
        public const string DistanceUnitProperty = "distu";
        public const string ExternalRecordIdProperty = "extrid";
        public const string DataSourceProperty = "dsrc";
        public const string LastUpdatedOnProperty = "uon";
        public const string LastUpdatedByProperty = "uby";
        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedOnProperty = "rcon";

        [BsonElement(IdProperty)]
        public ObjectId Id { get; set; }

        [BsonElement(ContactIdProperty)]
        public ObjectId ContactId { get; set; }

        [BsonElement(RoleIdProperty)]
        public ObjectId? RoleId { get; set; }

        [BsonElement(CustomRoleNameProperty)]
        public string CustomRoleName { get; set; }

        [BsonElement(StartDateProperty)]
        public DateTime? StartDate { get; set; }

        [BsonElement(EndDateProperty)]
        public DateTime? EndDate { get; set; }
        
        [BsonElement(CoreProperty)]
        public bool Core { get; set; }

        [BsonElement(NotesProperty)]
        public string Notes { get; set; }

        [BsonElement(FrequencyProperty)]
        public ObjectId? Frequency { get; set; }

        [BsonElement(DistanceProperty)]
        public int? Distance { get; set; }

        [BsonElement(DistanceUnitProperty)]
        public string DistanceUnit { get; set; }

        [BsonElement(ExternalRecordIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ExternalRecordId { get; set; }
        
        [BsonElement(DataSourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string DataSource { get; set; }

        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public CareTeamMemberStatus Status { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime RecordCreatedOn { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(LastUpdatedByProperty)]
        public ObjectId? UpdatedBy { get; set; }

        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime? LastUpdatedOn { get; set; }
    }
}
