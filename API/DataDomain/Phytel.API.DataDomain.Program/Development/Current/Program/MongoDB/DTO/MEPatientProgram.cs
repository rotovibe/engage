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
    public class MEPatientProgram : MEProgramBase, IMongoEntity<ObjectId>
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

        public const string EligibilityProperty = "eligibility";
        [BsonElement(EligibilityProperty)]
        public GenericStatus Eligibility { get; set; }

        public const string EligibilityOverrideProperty = "eligoverride";
        [BsonElement(EligibilityOverrideProperty)]
        public GenericSetting EligibilityOverride { get; set; }

        public const string EnrollmentProperty = "enrollment";
        [BsonElement(EnrollmentProperty)]
        public GenericStatus Enrollment { get; set; }

        public const string GraduatedFlagProperty = "graduated";
        [BsonElement(GraduatedFlagProperty)]
        public bool GraduatedFlag { get; set; }

        public const string IneligibleReasonProperty = "ineligible";
        [BsonElement(IneligibleReasonProperty)]
        public string IneligibleReason { get; set; }

        public const string OptOutProperty = "opt-out";
        [BsonElement(OptOutProperty)]
        public string OptOut { get; set; }

        public const string OptOutReasonProperty = "optoutreason";
        [BsonElement(OptOutReasonProperty)]
        public string OptOutReason { get; set; }

        public const string OptOutDateProperty = "optoutdate";
        [BsonElement(OptOutDateProperty)]
        public DateTime? OptOutDate { get; set; }

        public const string RemovedReasonProperty = "removedreason";
        [BsonElement(RemovedReasonProperty)]
        public string RemovedReason { get; set; }

        public const string DidNotEnrollReasonProperty = "notenrollreason";
        [BsonElement(DidNotEnrollReasonProperty)]
        public string NotEnrollReason { get; set; }

        public const string OverrideReasonProperty = "overridereason";
        [BsonElement(OverrideReasonProperty)]
        public string OverrideReason { get; set; }

        public const string TTLDateProperty = "ttl";
        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? TTLDate { get; set; }
    }
}
