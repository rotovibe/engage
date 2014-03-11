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
    [MongoIndex(Keys = new string[] { PatientGoalIdProperty, DeleteFlagProperty }, Unique = false)]
    public class MEPatientTask : GoalBase, IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatientTask() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string PatientGoalIdProperty = "pgid";
        [BsonElement(PatientGoalIdProperty)]
        [BsonIgnoreIfNull(false)]
        public ObjectId PatientGoalId { get; set; }

        public const string TargetValueProperty = "tv";
        [BsonElement(TargetValueProperty)]
        [BsonIgnoreIfNull(false)]
        public string TargetValue { get; set; }

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
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
        public ObjectId UpdatedBy { get; set; }

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
