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
    public class MEProgramBase : MEPlanElement
    {
        public MEProgramBase() {}

        public const string NameProperty = "nm";
        public const string DescriptionProperty = "dsc";
        public const string StartDateProperty = "sd";
        public const string EndDateProperty = "ed";
        public const string ObjectivesInfoProperty = "obj";
        public const string AuthoredByProperty = "athby";
        public const string LockedProperty = "lck";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string LastUpdatedOnProperty = "uon";

        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }

        public const string ShortNameProperty = "sn";
        [BsonElement(ShortNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string ShortName { get; set; }

        [BsonElement(DescriptionProperty)]
        [BsonIgnoreIfNull(true)]
        public string Description { get; set; }

        public const string ClientProperty = "cli";
        [BsonElement(ClientProperty)]
        [BsonIgnoreIfNull(true)]
        public string Client { get; set; }

        [BsonElement(StartDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? StartDate { get; set; }

        [BsonElement(EndDateProperty)] 
        [BsonIgnoreIfNull(false)]
        public DateTime? EndDate { get; set; }

        public const string EligibilityRequirementsProperty = "er";
        [BsonElement(EligibilityRequirementsProperty)]
        [BsonIgnoreIfNull(true)]
        public string EligibilityRequirements { get; set; }

        public const string EligibilityStartDateProperty = "esd";
        [BsonElement(EligibilityStartDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? EligibilityStartDate { get; set; }

        public const string EligibilityEndDateProperty = "eedt";
        [BsonElement(EligibilityEndDateProperty)]
        [BsonIgnoreIfNull(false)]
        public DateTime? EligibilityEndDate { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status Status { get; set; }

        [BsonElement(ObjectivesInfoProperty)]
        [BsonIgnoreIfNull(true)]
        public List<Objectives> ObjectivesInfo { get; set; }
        //public List<string> Attributes { get; set; }

        public const string ModulesProperty = "ms";
        [BsonElement(ModulesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MEModules> Modules { get; set; }

        [BsonElement(AuthoredByProperty)]
        [BsonIgnoreIfNull(true)]
        public string AuthoredBy { get; set; }

        [BsonElement(LockedProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Locked { get; set; }

        public const string ExtraElementsProperty = "ex";
        [BsonElement(ExtraElementsProperty)]
        [BsonExtraElements()]
        [BsonIgnoreIfNull(true)]
        public Dictionary<string, object> ExtraElements { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue("v1")]
        public string Version { get; set; }

        [BsonElement(UpdatedByProperty)]
        [BsonDefaultValue("-100")]
        public string UpdatedBy { get; set; }

        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? LastUpdatedOn { get; set; }
    }

    public class MEModules : MEPlanElement, IMongoEntity<ObjectId>
    {
        public MEModules() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string ProgramIdProperty = "progid";
        [BsonElement(ProgramIdProperty)]
        public ObjectId ProgramId { get; set; }

        public const string NameProperty = "nm";
        [BsonElement(NameProperty)]
        public string Name { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        public const string ObjectivesProperty = "obj";
        [BsonElement(ObjectivesProperty)]
        public List<Objectives> Objectives { get; set; }

        public const string ActionsProperty = "acts";
        [BsonElement(ActionsProperty)]
        public List<MEAction> Actions { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }

    public class MEAction : MEPlanElement, IMongoEntity<ObjectId>
    {
        public MEAction() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string ModuleIdProperty = "mid";
        [BsonElement(ModuleIdProperty)]
        public ObjectId ModuleId { get; set; }

        public const string NameProperty = "nm";
        [BsonElement(NameProperty)]
        public string Name { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        public const string ObjectivesProperty = "obj";
        [BsonElement(ObjectivesProperty)]
        public List<Objectives> Objectives { get; set; }

        public const string StepsProperty = "sps";
        [BsonElement(StepsProperty)]
        public List<MEStep> Steps { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }

    public class MEStep : MEPlanElement, IMongoEntity<ObjectId>
    {
        public MEStep() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        public const string ActionIdProperty = "actid";
        public const string HeaderProperty = "hdr";
        public const string QuestionProperty = "q";
        public const string ExProperty = "ex";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(ActionIdProperty)]
        public ObjectId ActionId { get; set; }

        public const string TypeProperty = "type";
        [BsonElement(TypeProperty)]
        public int StepTypeId { get; set; }

        [BsonElement(HeaderProperty)]
        public string Header { get; set; }

        public const string SelectedResponseIdProperty = "srid";
        [BsonElement(SelectedResponseIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string SelectedResponseId { get; set; }

        public const string ControlTypeProperty = "ctype";
        [BsonElement(ControlTypeProperty)]
        public int ControlType { get; set; }

        public const string SelectTypeProperty = "selt";
        [BsonElement(SelectTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public int SelectType { get; set; }

        public const string IncludeTimeProperty = "it";
        [BsonElement(IncludeTimeProperty)]
        [BsonIgnoreIfNull(true)]
        public bool IncludeTime { get; set; }

        [BsonElement(QuestionProperty)]
        public string Question { get; set; }

        public const string TProperty = "t";
        [BsonElement(TProperty)]
        [BsonIgnoreIfNull(true)]
        public string Title { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        public const string NotesProperty = "nts";
        [BsonElement(NotesProperty)]
        [BsonIgnoreIfNull(true)]
        public string Notes { get; set; }

        public const string TextProperty = "txt";
        [BsonElement(TextProperty)]
        public string Text { get; set; }

        [BsonElement(ExProperty)]
        public string Ex { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }

        public const string ResponsesProperty = "resp";
        [BsonElement(ResponsesProperty)]
        public List<ResponseInfo> Responses { get; set; }
    }

    public class ResponseInfo : IMongoEntity<ObjectId>
    {
        public ResponseInfo() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        public const string TextProperty = "txt";
        public const string RequiredProperty = "req";

        [BsonId]
        public ObjectId Id { get; set; }

        public const string OrderProperty = "o";
        [BsonElement(OrderProperty)]
        public int Order { get; set; }

        [BsonElement(TextProperty)]
        public string Text { get; set; }

        public const string StepIdProperty = "sid";
        [BsonElement(StepIdProperty)]
        public ObjectId StepId { get; set; }

        public const string ValueProperty = "val";
        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        public const string NominalProperty = "nml";
        [BsonElement(NominalProperty)]
        public bool Nominal { get; set; }

        [BsonElement(RequiredProperty)]
        public bool Required { get; set; }

        public const string NextStepIdProperty = "nsid";
        [BsonElement(NextStepIdProperty)]
        public ObjectId NextStepId { get; set; }

        public const string SpawnElementProperty = "spwn";
        [BsonElement(SpawnElementProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MESpawnElement> Spawn { get; set; }
    }

    public class Objectives : IMongoEntity<ObjectId>
    {
        public Objectives() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";

        [BsonId]
        public ObjectId Id { get; set; }

        public const string ValueProperty = "val";
        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        public const string UnitProperty = "unit";
        [BsonElement(UnitProperty)]
        public string Unit { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }
}
