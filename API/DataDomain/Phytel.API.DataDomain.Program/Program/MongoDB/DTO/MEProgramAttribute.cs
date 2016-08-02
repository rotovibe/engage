using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;
using Phytel.API.DataDomain.Program;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { PlanElementIdProperty }, Unique = false)]
    public class MEProgramAttribute : IMEEntity, IMongoEntity<ObjectId>
    {
        public MEProgramAttribute(string userId)
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

        public const string PlanElementIdProperty = "peid";
        [BsonElement(PlanElementIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId PlanElementId { get; set; }

        //public const string EndDateProperty = "ed"; , Sprint 12
        //[BsonElement(EndDateProperty)]
        //[BsonIgnoreIfNull(false)]
        //public DateTime? EndDate { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status Status { get; set; }

        public const string LockedProperty = "lck";
        [BsonElement(LockedProperty)]
        [BsonIgnoreIfNull(true)]
        public Locked Locked { get; set; }

        public const string IneligibleReasonProperty = "ir";
        [BsonElement(IneligibleReasonProperty)]
        public string IneligibleReason { get; set; }

        public const string EligibilityProperty = "elg";
        [BsonElement(EligibilityProperty)]
        public EligibilityStatus Eligibility { get; set; }

        //public const string EligibilityOverrideProperty = "eo";
        //[BsonElement(EligibilityOverrideProperty)]
        //public EligibilityOverride EligibilityOverride { get; set; }

        public const string EnrollmentProperty = "enr";
        [BsonElement(EnrollmentProperty)]
        public EnrollmentStatus Enrollment { get; set; }

        public const string GraduatedFlagProperty = "gf";
        [BsonElement(GraduatedFlagProperty)]
        public Graduated GraduatedFlag { get; set; }

        public const string OptOutProperty = "oo";
        [BsonElement(OptOutProperty)]
        public bool OptOut { get; set; }

        //public const string OptOutReasonProperty = "oor";
        //[BsonElement(OptOutReasonProperty)]
        //public string OptOutReason { get; set; }

        //public const string OptOutDateProperty = "ood";
        //[BsonElement(OptOutDateProperty)]
        //public DateTime? OptOutDate { get; set; }

        public const string PopulationProperty = "pop";
        [BsonElement(PopulationProperty)]
        [BsonIgnoreIfNull(false)]
        public string Population { get; set; }

        public const string RemovedReasonProperty = "rr";
        [BsonElement(RemovedReasonProperty)]
        public string RemovedReason { get; set; }

        public const string DidNotEnrollReasonProperty = "dner";
        [BsonElement(DidNotEnrollReasonProperty)]
        public string DidNotEnrollReason { get; set; }

        //public const string DisEnrollReasonProperty = "der";
        //[BsonElement(DisEnrollReasonProperty)]
        //public string DisEnrollReason { get; set; }

        public const string OverrideReasonProperty = "or";
        [BsonElement(OverrideReasonProperty)]
        public string OverrideReason { get; set; }

        public const string StartDateProperty = "sd"; //, Sprint 12
        [BsonElement(StartDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? StartDate { get; set; }

        public const string AssignDateProperty = "aon";
        [BsonElement(AssignDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? AssignedOn { get; set; }

        public const string AssignByProperty = "aby";
        [BsonElement(AssignByProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? AssignedBy { get; set; }

        public const string AssignToProperty = "ato";
        [BsonElement(AssignToProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? AssignedTo { get; set; }

        public const string CompletedByProperty = "cby";
        [BsonElement(CompletedByProperty)]
        [BsonIgnoreIfNull(true)]
        public string CompletedBy { get; set; }

        public const string CompletedProperty = "cpld";
        [BsonElement(CompletedProperty)]
        [BsonIgnoreIfNull(false)]
        public Completed Completed { get; set; }

        public const string CompletedOnProperty = "dc";
        [BsonElement(CompletedOnProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? DateCompleted { get; set; }

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? UpdatedBy { get; set; }

        public const string DeleteFlagProperty = "del";
        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        public const string TTLDateProperty = "ttl";
        [BsonElement(TTLDateProperty)]
        [BsonDefaultValue(null)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime? TTLDate { get; set; }

        public const string LastUpdatedOnProperty = "uon";
        [BsonIgnoreIfNull(true)]
        [BsonElement(LastUpdatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime? LastUpdatedOn { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; private set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; private set; }
    }
}
