using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEPlanElement : IMEEntity
    {
        public MEPlanElement() { }

        public const string StartDateProperty = "sd";
        [BsonElement(StartDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? StartDate { get; set; }

        public const string SourceIdProperty = "srcid";
        [BsonElement(SourceIdProperty)]
        [BsonIgnoreIfNull(false)]
        public string SourceId { get; set; }

        public const string OrderProperty = "o";
        [BsonElement(OrderProperty)]
        [BsonIgnoreIfNull(false)]
        public int Order { get; set; }

        public const string EnabledProperty = "e";
        [BsonElement(EnabledProperty)]
        [BsonIgnoreIfNull(false)]
        public bool Enabled { get; set; }

        public const string StateProperty = "st";
        [BsonElement(StateProperty)]
        [BsonIgnoreIfNull(true)]
        public ElementState State { get; set; }

        public const string AssignDateProperty = "aon";
        [BsonElement(AssignDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? AssignedOn { get; set; }

        public const string AssignByProperty = "aby";
        [BsonElement(AssignByProperty)]
        [BsonIgnoreIfNull(true)]
        public string AssignedBy { get; set; }

        public const string CompletedByProperty = "cby";
        [BsonElement(CompletedByProperty)]
        [BsonIgnoreIfNull(true)]
        public string CompletedBy { get; set; }

        public const string CompletedProperty = "cpld";
        [BsonElement(CompletedProperty)]
        [BsonIgnoreIfNull(false)]
        public bool Completed { get; set; }

        public const string CompletedOnProperty = "con";
        [BsonElement(CompletedOnProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? DateCompleted { get; set; }

        public const string NextProperty = "nxt";
        [BsonElement(NextProperty)]
        [BsonIgnoreIfNull(true)]
        public string Next { get; set; }

        public const string PreviousProperty = "prev";
        [BsonElement(PreviousProperty)]
        [BsonIgnoreIfNull(true)]
        public string Previous { get; set; }

        public const string SpawnProperty = "spwn";
        [BsonElement(SpawnProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MESpawnElement> Spawn { get; set; }

        public const string ExtraElementsProperty = "ex";
        [BsonExtraElements]
        [BsonIgnoreIfNull(true)]
        [BsonElement(ExtraElementsProperty)]
        public Dictionary<string, object> ExtraElements { get; set; }

        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue("v1")]
        public string Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
        [BsonIgnoreIfNull(true)]
        public string UpdatedBy { get; set; }

        public const string DeleteFlagProperty = "del";
        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        public const string TTLDateProperty = "ttl";
        [BsonElement(TTLDateProperty)]
        [BsonDefaultValue(null)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? TTLDate { get; set; }

        public const string LastUpdatedOnProperty = "uon";
        [BsonIgnoreIfNull(true)]
        [BsonElement(LastUpdatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? LastUpdatedOn { get; set; }
    }
}