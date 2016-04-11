using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.MongoDB.DTO
{
    public class CareTeamMember : IMEDataSource
    {

        public const string IDProperty = "_id";
        public const string ContactIdProperty = "cid";
        public const string RoleIdProperty = "rid";
        public const string CustomRoleNameProperty = "crnm";
        public const string StartDateProperty = "sd";
        public const string EndDateProperty = "ed";
        public const string CoreProperty = "cor";
        public const string NotesProperty = "nts";
        public const string StatusProperty = "sts";
        public const string FrequencyProperty = "freq";
        public const string DistanceProperty = "dist";
        public const string ExternalRecordIdProperty = "extrid";
        public const string DataSourceProperty = "dsrc";

        [BsonElement(IDProperty)]
        public ObjectId Id { get; set; }

        [BsonElement(ContactIdProperty)]
        public ObjectId ContactId { get; set; }

        [BsonElement(ContactIdProperty)]
        public ObjectId RoleId { get; set; }

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
        public int Distance { get; set; }
       
        [BsonElement(ExternalRecordIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ExternalRecordId { get; set; }
        
        [BsonElement(DataSourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string DataSource { get; set; }
    }
}
