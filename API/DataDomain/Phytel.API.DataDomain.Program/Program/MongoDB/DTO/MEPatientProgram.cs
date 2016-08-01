using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { PatientIdProperty, ContractProgramIdProperty, StateProperty }, Unique = false)]
    public class MEPatientProgram : ProgramBase, IMEEntity, IMongoEntity<ObjectId>
    {
        public MEPatientProgram(string userId)
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

        public const string PatientIdProperty = "pid";
        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId PatientId { get; set; }

        public const string ContractProgramIdProperty = "cpid";
        [BsonElement(ContractProgramIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId ContractProgramId { get; set; }

        // depricated - Use Element state instead.
        //public const string ProgramStateProperty = "progstate";
        //[BsonElement(ProgramStateProperty)]
        //public ProgramState ProgramState { get; set; }

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

        #region // will be moved to other collection
        //public const string IneligibleReasonProperty = "ir";
        //[BsonElement(IneligibleReasonProperty)]
        //public string IneligibleReason { get; set; }

        //public const string EligibilityProperty = "elg";
        //[BsonElement(EligibilityProperty)]
        //public EligibilityStatus Eligibility { get; set; }

        //public const string EligibilityOverrideProperty = "eo";
        //[BsonElement(EligibilityOverrideProperty)]
        //public GenericSetting EligibilityOverride { get; set; }

        //public const string EnrollmentProperty = "enr";
        //[BsonElement(EnrollmentProperty)]
        //public GenericStatus Enrollment { get; set; }

        //public const string GraduatedFlagProperty = "gf";
        //[BsonElement(GraduatedFlagProperty)]
        //public bool GraduatedFlag { get; set; }

        //public const string OptOutProperty = "oo";
        //[BsonElement(OptOutProperty)]
        //public string OptOut { get; set; }

        //public const string OptOutReasonProperty = "oor";
        //[BsonElement(OptOutReasonProperty)]
        //public string OptOutReason { get; set; }

        //public const string OptOutDateProperty = "ood";
        //[BsonElement(OptOutDateProperty)]
        //public DateTime? OptOutDate { get; set; }

        //public const string PopulationProperty = "pop";
        //[BsonElement(PopulationProperty)]
        //[BsonIgnoreIfNull(false)]
        //public string Population { get; set; }

        //public const string RemovedReasonProperty = "rr";
        //[BsonElement(RemovedReasonProperty)]
        //public string RemovedReason { get; set; }

        //public const string DidNotEnrollReasonProperty = "dner";
        //[BsonElement(DidNotEnrollReasonProperty)]
        //public string DidNotEnrollReason { get; set; }

        //public const string DisEnrollReasonProperty = "der";
        //[BsonElement(DisEnrollReasonProperty)]
        //public string DisEnrollReason { get; set; }

        //public const string OverrideReasonProperty = "or";
        //[BsonElement(OverrideReasonProperty)]
        //public string OverrideReason { get; set; }
        #endregion
    }
}
