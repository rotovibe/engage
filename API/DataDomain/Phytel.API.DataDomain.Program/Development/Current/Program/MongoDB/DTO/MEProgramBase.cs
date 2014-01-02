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
    public class MEProgramBase
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

    public class Modules : IMongoEntity<ObjectId>
    {
        public Modules() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string NameProperty = "nm";
        [BsonElement(NameProperty)]
        public string Name { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        public const string ObjectivesProperty = "objs";
        [BsonElement(ObjectivesProperty)]
        public List<ObjectivesInfo> Objectives { get; set; }

        public const string ActionsProperty = "actions";
        [BsonElement(ActionsProperty)]
        public List<ActionsInfo> Actions { get; set; }

        public const string StatusProperty = "status";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }

    public class ActionsInfo : IMongoEntity<ObjectId>
    {
        public ActionsInfo() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string NameProperty = "nm";
        [BsonElement(NameProperty)]
        public string Name { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        public const string CompletedByProperty = "cmplby";
        [BsonElement(CompletedByProperty)]
        public string CompletedBy { get; set; }

        public const string ObjectivesProperty = "objs";
        [BsonElement(ObjectivesProperty)]
        public List<ObjectivesInfo> Objectives { get; set; }

        public const string StepsProperty = "steps";
        [BsonElement(StepsProperty)]
        public List<StepsInfo> Steps { get; set; }

        public const string StatusProperty = "status";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }

    public class StepsInfo : IMongoEntity<ObjectId>
    {
        public StepsInfo() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string TypeProperty = "typ";
        [BsonElement(TypeProperty)]
        public int Type { get; set; }

        public const string QuestionProperty = "q";
        [BsonElement(QuestionProperty)]
        public string Question { get; set; }

        public const string TProperty = "t";
        [BsonElement(TProperty)]
        public string T { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        public const string NotesProperty = "notes";
        [BsonElement(NotesProperty)]
        public string Notes { get; set; }

        public const string TextProperty = "txt";
        [BsonElement(TextProperty)]
        public string Text { get; set; }

        public const string ExProperty = "ex";
        [BsonElement(ExProperty)]
        public string Ex { get; set; }

        public const string StatusProperty = "status";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }

    public class ObjectivesInfo : IMongoEntity<ObjectId>
    {
        public ObjectivesInfo() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string ValueProperty = "val";
        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        public const string UnitProperty = "unit";
        [BsonElement(UnitProperty)]
        public string Unit { get; set; }

        public const string StatusProperty = "status";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }
}
