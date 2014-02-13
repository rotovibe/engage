using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEPatientTask : GoalBase, IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatientTask() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string TargetValueProperty = "tv";
        [BsonElement(TargetValueProperty)]
        [BsonIgnoreIfNull(false)]
        public string TargetValue { get; set; }

        public const string OrderProperty = "o";
        [BsonElement(OrderProperty)]
        [BsonIgnoreIfNull(false)]
        public int Order { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public GoalTaskStatus Status { get; set; }

        public const string TargetDateProperty = "td";
        [BsonElement(TargetDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? TargetDate { get; set; }

        public const string AttributesProperty = "attr";
        [BsonElement(AttributesProperty)]
        [BsonIgnoreIfNull(false)]
        public List<MAttribute> Attributes { get; set; }

        public const string BarriersProperty = "bar";
        [BsonElement(BarriersProperty)]
        [BsonIgnoreIfNull(false)]
        public List<ObjectId> Barriers { get; set; }

        #region Standard IMongoEntity Implementation
        public const string ExtraElementsProperty = "ex";
        [BsonElement(ExtraElementsProperty)]
        [BsonExtraElements()]
        [BsonIgnoreIfNull(true)]
        public Dictionary<string, object> ExtraElements { get; set; }

        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue("v1")]
        public string Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
        [BsonDefaultValue("-100")]
        public string UpdatedBy { get; set; }

        public const string DeleteFlagProperty = "del";
        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        public const string TTLDateProperty = "ttl";
        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public System.DateTime? TTLDate { get; set; }

        public const string LastUpdatedOnProperty = "uon";
        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public System.DateTime? LastUpdatedOn { get; set; }
        #endregion
    }
}
