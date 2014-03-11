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
    [MongoIndex(Keys = new string[] { PatientIdProperty }, Unique = false)]
    [MongoIndex(Keys = new string[] { ContractProgramIdProperty }, Unique = false)]
    public class MEPatientProgram : ProgramBase, IMEEntity, IMongoEntity<ObjectId>
    {
        public MEPatientProgram() { Id = ObjectId.GenerateNewId(); }

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

        public const string ProgramStateProperty = "progstate";
        [BsonElement(ProgramStateProperty)]
        public ProgramState ProgramState { get; set; }

        public const string ExtraElementsProperty = "ex";
        [BsonExtraElements]
        [BsonIgnoreIfNull(true)]
        [BsonElement(ExtraElementsProperty)]
        public Dictionary<string, object> ExtraElements { get; set; }

        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId UpdatedBy { get; set; }

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
