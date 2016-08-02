using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Phytel.API.DataDomain.Scheduling.DTO;

namespace Phytel.API.DataDomain.Scheduling
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { IdProperty, DeleteFlagProperty }, Unique = false)]
    public class MESchedule : IMongoEntity<ObjectId>, IMEEntity
    {
        public MESchedule(string userId)
        {
            Id = ObjectId.GenerateNewId();
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = System.DateTime.UtcNow;
            Version = 1.0;
        }

        public const string IdProperty = "_id";
        public const string PatientIdProperty = "pid";
        public const string AssignedToProperty = "ato";
        public const string DescriptionProperty = "desc";
        public const string TitleProperty = "t";
        public const string ProgramProperty = "prog";
        public const string StatusProperty = "sts";
        public const string CatgegoryProperty = "cat";
        public const string PriorityProperty = "pri";
        public const string DueDateProperty = "dd";
        public const string StartTimeProperty = "sd";
        public const string DurationProperty = "dur";
        public const string DueDateRangeProperty = "ddr";
        public const string ClosedDateProperty = "cd";
        public const string SchedulingTypeProperty = "type";
        public const string DefaultAssignmentProperty = "dasgn";

        #region Standard IMongoEntity Constants
        public const string ExtraElementsProperty = "ex";
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
        public ObjectId? PatientId { get; set; }

        [BsonElement(AssignedToProperty)]
        [BsonIgnoreIfNull(false)]
        public ObjectId? AssignedToId { get; set; }

        [BsonElement(DescriptionProperty)]
        [BsonIgnoreIfNull(true)]
        public string Description { get; set; }

        [BsonElement(TitleProperty)]
        [BsonIgnoreIfNull(true)]
        public string Title { get; set; }

        [BsonElement(ProgramProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> ProgramIds { get; set; }

        [BsonElement(DueDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? DueDate { get; set; }

        [BsonElement(DueDateRangeProperty)]
        [BsonIgnoreIfNull(true)]
        public int DueDateRange { get; set; }

        [BsonElement(StartTimeProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? StartTime { get; set; }

        [BsonElement(DurationProperty)]
        [BsonIgnoreIfNull(true)]
        public int? Duration { get; set; }

        [BsonElement(ClosedDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? ClosedDate { get; set; }

        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status Status { get; set; }

        [BsonElement(CatgegoryProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? Category { get; set; }

        [BsonElement(PriorityProperty)]
        [BsonIgnoreIfNull(true)]
        public Priority Priority { get; set; }

        [BsonElement(SchedulingTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public ScheduleType Type { get; set; }

        [BsonElement(DefaultAssignmentProperty)]
        [BsonIgnoreIfNull(true)]
        public bool DefaultAssignment { get; set; }

        #region Standard IMongoEntity Implementation

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

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }
    }
}
