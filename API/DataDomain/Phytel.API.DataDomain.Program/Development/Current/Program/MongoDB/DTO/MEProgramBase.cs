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
        public const string ShortNameProperty = "snm";
        public const string DescriptionProperty = "dsc";
        public const string ClientProperty = "cp";
        public const string StartDateProperty = "sd";
        public const string EndDateProperty = "ed";
        public const string EligibilityRequirementsProperty = "elr";
        public const string EligibilityStartDateProperty = "elsd";
        public const string EligibilityEndDateProperty = "eled";
        public const string StatusProperty = "status";
        public const string ObjectivesInfoProperty = "objs";
        public const string ModulesProperty = "modules";
        public const string AuthoredByProperty = "athby";
        public const string LockedProperty = "lck";
        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string LastUpdatedOnProperty = "uon";

        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }

        [BsonElement(ShortNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string ShortName { get; set; }

        [BsonElement(DescriptionProperty)]
        [BsonIgnoreIfNull(true)]
        public string Description { get; set; }

        [BsonElement(ClientProperty)]
        [BsonIgnoreIfNull(true)]
        public string Client { get; set; }

        [BsonElement(StartDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? StartDate { get; set; }

        [BsonElement(EndDateProperty)] 
        [BsonIgnoreIfNull(false)]
        public DateTime? EndDate { get; set; }

        [BsonElement(EligibilityRequirementsProperty)]
        [BsonIgnoreIfNull(true)]
        public string EligibilityRequirements { get; set; }

        [BsonElement(EligibilityStartDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? EligibilityStartDate { get; set; }

        [BsonElement(EligibilityEndDateProperty)]
        [BsonIgnoreIfNull(false)]
        public DateTime? EligibilityEndDate { get; set; }

        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status Status { get; set; }

        [BsonElement(ObjectivesInfoProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectivesInfo> ObjectivesInfo { get; set; }
        //public List<string> Attributes { get; set; }

        [BsonElement(ModulesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<Modules> Modules { get; set; }

        [BsonElement(AuthoredByProperty)]
        [BsonIgnoreIfNull(true)]
        public string AuthoredBy { get; set; }

        [BsonElement(LockedProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Locked { get; set; }

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

    public class Modules : MEPlanElement, IMongoEntity<ObjectId>
    {
        public Modules() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        public const string ProgramIdProperty = "progid";
        public const string NameProperty = "nm";
        public const string DescriptionProperty = "desc";
        public const string ObjectivesProperty = "objs";
        public const string ActionsProperty = "actions";
        public const string StatusProperty = "status";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(ProgramIdProperty)]
        public ObjectId ProgramId { get; set; }

        [BsonElement(NameProperty)]
        public string Name { get; set; }

        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        [BsonElement(ObjectivesProperty)]
        public List<ObjectivesInfo> Objectives { get; set; }

        [BsonElement(ActionsProperty)]
        public List<ActionsInfo> Actions { get; set; }

        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }

    public class ActionsInfo : MEPlanElement, IMongoEntity<ObjectId>
    {
        public ActionsInfo() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        public const string ModuleIdProperty = "moduleid";
        public const string NameProperty = "nm";
        public const string DescriptionProperty = "desc";
        public const string CompletedByProperty = "cmplby";
        public const string ObjectivesProperty = "objs";
        public const string StepsProperty = "steps";
        public const string StatusProperty = "status";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(ModuleIdProperty)]
        public ObjectId ModuleId { get; set; }

        [BsonElement(NameProperty)]
        public string Name { get; set; }

        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        [BsonElement(CompletedByProperty)]
        public string CompletedBy { get; set; }

        [BsonElement(ObjectivesProperty)]
        public List<ObjectivesInfo> Objectives { get; set; }

        [BsonElement(StepsProperty)]
        public List<StepsInfo> Steps { get; set; }

        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }

    public class StepsInfo : MEPlanElement, IMongoEntity<ObjectId>
    {
        public StepsInfo() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        public const string ActionIdProperty = "actid";
        public const string TypeProperty = "typ";
        public const string HeaderProperty = "hdr";
        public const string SelectedResponseIdProperty = "selectedresp";
        public const string ControlTypeProperty = "controltype";
        public const string SelectTypeProperty = "selecttype";
        public const string IncludeTimeProperty = "inctime";
        public const string QuestionProperty = "q";
        public const string TProperty = "t";
        public const string DescriptionProperty = "desc";
        public const string NotesProperty = "notes";
        public const string TextProperty = "txt";
        public const string ExProperty = "ex";
        public const string StatusProperty = "status";
        public const string ResponsesProperty = "responses";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(ActionIdProperty)]
        public ObjectId ActionId { get; set; }

        [BsonElement(TypeProperty)]
        public int StepTypeId { get; set; }

        [BsonElement(HeaderProperty)]
        public string Header { get; set; }

        [BsonElement(SelectedResponseIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string SelectedResponseId { get; set; }

        [BsonElement(ControlTypeProperty)]
        public int ControlType { get; set; }

        [BsonElement(SelectTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public int SelectType { get; set; }

        [BsonElement(IncludeTimeProperty)]
        [BsonIgnoreIfNull(true)]
        public bool IncludeTime { get; set; }

        [BsonElement(QuestionProperty)]
        public string Question { get; set; }

        [BsonElement(TProperty)]
        [BsonIgnoreIfNull(true)]
        public string Title { get; set; }

        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        [BsonElement(NotesProperty)]
        [BsonIgnoreIfNull(true)]
        public string Notes { get; set; }

        [BsonElement(TextProperty)]
        public string Text { get; set; }

        [BsonElement(ExProperty)]
        public string Ex { get; set; }

        [BsonElement(StatusProperty)]
        public Status Status { get; set; }

        [BsonElement(ResponsesProperty)]
        public List<ResponseInfo> Responses { get; set; }
    }

    public class ResponseInfo : IMongoEntity<ObjectId>
    {
        public ResponseInfo() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        public const string OrderProperty = "order";
        public const string TextProperty = "txt";
        public const string StepIdProperty = "stepid";
        public const string ValueProperty = "value";
        public const string NominalProperty = "nominal";
        public const string RequiredProperty = "req";
        public const string NextStepIdProperty = "nextstepid";
        public const string SpawnElementProperty = "spawn";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(OrderProperty)]
        public int Order { get; set; }

        [BsonElement(TextProperty)]
        public string Text { get; set; }

        [BsonElement(StepIdProperty)]
        public ObjectId StepId { get; set; }

        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        [BsonElement(NominalProperty)]
        public bool Nominal { get; set; }

        [BsonElement(RequiredProperty)]
        public bool Required { get; set; }

        [BsonElement(NextStepIdProperty)]
        public ObjectId NextStepId { get; set; }

        [BsonElement(SpawnElementProperty)]
        [BsonIgnoreIfNull(true)]
        public MESpawnElement SpawnElement { get; set; }
    }

    public class ObjectivesInfo : IMongoEntity<ObjectId>
    {
        public ObjectivesInfo() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        public const string ValueProperty = "val";
        public const string UnitProperty = "unit";
        public const string StatusProperty = "status";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        [BsonElement(UnitProperty)]
        public string Unit { get; set; }

        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }
}
