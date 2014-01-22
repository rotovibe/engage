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
        public const string PatientIdProperty = "pid";
        public const string ContractProgramIdProperty = "cpid";
        public const string ProgramStateProperty = "progstate";
        public const string PopulationProperty = "population";
        public const string EligibilityProperty = "eligibility";
        public const string EligibilityOverrideProperty = "eligoverride";
        public const string EnrollmentProperty = "enrollment";
        public const string GraduatedFlagProperty = "graduated";
        public const string IneligibleReasonProperty = "ineligible";
        public const string OptOutProperty = "opt-out";
        public const string OptOutReasonProperty = "optoutreason";
        public const string OptOutDateProperty = "optoutdate";
        public const string RemovedReasonProperty = "removedreason";
        public const string DidNotEnrollReasonProperty = "notenrollreason";
        public const string DisEnrollReasonProperty = "disenrollreason";
        public const string OverrideReasonProperty = "overridereason";
        public const string TTLDateProperty = "ttl";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId PatientId { get; set; }

        [BsonElement(ContractProgramIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId ContractProgramId { get; set; }

        [BsonElement(ProgramStateProperty)]
        public ProgramState ProgramState { get; set; }

        [BsonElement(PopulationProperty)]
        [BsonIgnoreIfNull(false)]
        public string Population { get; set; }

        [BsonElement(EligibilityProperty)]
        public GenericStatus Eligibility { get; set; }

        [BsonElement(EligibilityOverrideProperty)]
        public GenericSetting EligibilityOverride { get; set; }

        [BsonElement(EnrollmentProperty)]
        public GenericStatus Enrollment { get; set; }

        [BsonElement(GraduatedFlagProperty)]
        public bool GraduatedFlag { get; set; }

        [BsonElement(IneligibleReasonProperty)]
        public string IneligibleReason { get; set; }

        [BsonElement(OptOutProperty)]
        public string OptOut { get; set; }

        [BsonElement(OptOutReasonProperty)]
        public string OptOutReason { get; set; }

        [BsonElement(OptOutDateProperty)]
        public DateTime? OptOutDate { get; set; }

        [BsonElement(RemovedReasonProperty)]
        public string RemovedReason { get; set; }

        [BsonElement(DidNotEnrollReasonProperty)]
        public string NotEnrollReason { get; set; }

        [BsonElement(DisEnrollReasonProperty)]
        public string DisEnrollReason { get; set; }

        [BsonElement(OverrideReasonProperty)]
        public string OverrideReason { get; set; }

        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? TTLDate { get; set; }
    }
}
