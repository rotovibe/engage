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
        public const string StatusProperty = "st";
        public const string ProgramStatusProperty = "pst";
        public const string ObjectivesInfoProperty = "oi";
        public const string ModulesProperty = "mods";
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
        [BsonIgnoreIfNull(true)]
        public DateTime? EndDate { get; set; }

        [BsonElement(EligibilityRequirementsProperty)]
        [BsonIgnoreIfNull(true)]
        public string EligibilityRequirements { get; set; }

        [BsonElement(EligibilityStartDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? EligibilityStartDate { get; set; }

        [BsonElement(EligibilityEndDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? EligibilityEndDate { get; set; }

        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status Status { get; set; }

        [BsonElement(ProgramStatusProperty)]
        [BsonIgnoreIfNull(true)]
        public string ProgramStatus { get; set; }

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
        public const string NameProperty = "nm";
        public const string DescriptionProperty = "desc";
        public const string ObjectivesProperty = "obj";
        public const string ActionsProperty = "acts";
        public const string StatusProperty = "stat";

        [BsonId]
        public ObjectId Id { get; set; }

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

    public class ActionsInfo : IMongoEntity<ObjectId>
    {
        public ActionsInfo() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        public const string NameProperty = "nm";
        public const string DescriptionProperty = "desc";
        public const string CompletedByProperty = "cmplby";
        public const string ObjectivesProperty = "objs";
        public const string StepsProperty = "stps";
        public const string StatusProperty = "stat";

        [BsonId]
        public ObjectId Id { get; set; }

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

    public class StepsInfo : IMongoEntity<ObjectId>
    {
        public StepsInfo() { Id = ObjectId.GenerateNewId(); }
        public const string IDProperty = "_id";
        public const string TypeProperty = "typ";
        public const string QuestionProperty = "q";
        public const string TProperty = "t";
        public const string DescriptionProperty = "desc";
        public const string NotesProperty = "notes";
        public const string TextProperty = "txt";
        public const string ExProperty = "ex";
        public const string StatusProperty = "stat";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(TypeProperty)]
        public int Type { get; set; }

        [BsonElement(QuestionProperty)]
        public string Question { get; set; }

        [BsonElement(TProperty)]
        public string T { get; set; }

        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        [BsonElement(NotesProperty)]
        public string Notes { get; set; }

        [BsonElement(TextProperty)]
        public string Text { get; set; }

        [BsonElement(ExProperty)]
        public string Ex { get; set; }

        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }

    public class ObjectivesInfo : IMongoEntity<ObjectId>
    {
        public ObjectivesInfo() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        public const string ValueProperty = "vl";
        public const string MeasurementProperty = "ms";
        public const string StatusProperty = "st";


        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        [BsonElement(MeasurementProperty)]
        public string Measurement { get; set; }

        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }
}
