using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Phytel.API.DataDomain.PatientObservation.MongoDB.DTO;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] {IdProperty, ObservationTypeProperty, StandardProperty, StatusProperty })]
    public class MEObservation : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEObservation(string userId)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = DateTime.UtcNow;
        }
        
        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedOnProperty = "rcon";

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string ObservationTypeProperty = "otype";
        [BsonElement(ObservationTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId ObservationTypeId { get; set; }

        public const string GroupIdProperty = "gid";
        [BsonElement(GroupIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? GroupId { get; set; }

        public const string CommonNameProperty = "cn";
        [BsonElement(CommonNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string CommonName { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        [BsonIgnoreIfNull(true)]
        public string Description { get; set; }

        public const string StandardProperty = "s";
        [BsonElement(StandardProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Standard { get; set; }

        public const string OrderProperty = "o";
        [BsonElement(OrderProperty)]
        [BsonIgnoreIfNull(true)]
        public int? Order { get; set; }

        public const string CodingSystemProperty = "csid";
        [BsonElement(CodingSystemProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? CodingSystemId { get; set; }

        public const string CodingSystemCodeProperty = "csc";
        [BsonElement(CodingSystemCodeProperty)]
        [BsonIgnoreIfNull(true)]
        public string Code { get; set; }

        public const string LowValueProperty = "lv";
        [BsonElement(LowValueProperty)]
        [BsonIgnoreIfNull(true)]
        public double? LowValue { get; set; }

        public const string HighValueProperty = "hv";
        [BsonElement(HighValueProperty)]
        [BsonIgnoreIfNull(true)]
        public double? HighValue { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status Status { get; set; }

        public const string UnitsProperty = "u";
        [BsonElement(UnitsProperty)]
        [BsonIgnoreIfNull(true)]
        public string Units { get; set; }

        public const string SourceProperty = "src";
        [BsonElement(SourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string Source { get; set; }

        #region Standard IMongoEntity Implementation
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

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

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; private set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; private set; }
        #endregion
    }


}
