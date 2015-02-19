using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { PatientIdProperty, DeleteFlagProperty }, Unique = false)]
    public class MEPatientGoal : GoalBase, IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatientGoal(string userId)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = DateTime.UtcNow;
        }

        public const string IdProperty = "_id";
        public const string PatientIdProperty = "pid";
        public const string FocusAreaProperty = "focs";
        public const string SourceProperty = "src";
        public const string ProgramProperty = "prog";
        public const string TypeProperty = "type";
        public const string EndDateProperty = "ed";
        public const string TargetValueProperty = "tv";
        public const string TargetDateProperty = "td";
        public const string AttributesProperty = "attr";        

        #region Standard IMongoEntity Constants
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";
        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedOnProperty = "rcon";
        #endregion

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId PatientId { get; set; }

        public const string TemplateIdProperty = "tid";
        [BsonElement(TemplateIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? TemplateId { get; set; }        

        [BsonElement(FocusAreaProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> FocusAreaIds { get; set; }
        
        [BsonElement(SourceProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? SourceId { get; set; }
        
        [BsonElement(ProgramProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> ProgramIds { get; set; }

        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(true)]
        public GoalType Type { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public GoalTaskStatus Status { get; set; }

        [BsonElement(EndDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? EndDate { get; set; }

        [BsonElement(TargetValueProperty)]
        [BsonIgnoreIfNull(true)]
        public string TargetValue { get; set; }

        [BsonElement(TargetDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? TargetDate { get; set; }

        [BsonElement(AttributesProperty)]
        [BsonIgnoreIfNull(false)]
        public List<MAttribute> Attributes { get; set; }

        #region Standard IMongoEntity Implementation
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
        #endregion
    }
}
