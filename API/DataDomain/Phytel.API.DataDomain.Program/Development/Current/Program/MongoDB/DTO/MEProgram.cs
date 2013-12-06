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
    public class MEProgram : IMongoEntity<ObjectId>, IMEEntity //, ISupportInitialize
    {
        public MEProgram() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string TemplateNameProperty = "ptn";
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
        public const string AuthoredByProperty = "athby";
        public const string LockedProperty = "lck";
        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(TemplateNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string TemplateName { get; set; }

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
        public string Status { get; set; }

        [BsonElement(ProgramStatusProperty)]
        [BsonIgnoreIfNull(true)]
        public string ProgramStatus { get; set; }

        [BsonElement(ObjectivesInfoProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectivesInfo> ObjectivesInfo { get; set; }
        //public List<string> Attributes { get; set; }

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

        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? TTLDate { get; set; }

        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? LastUpdatedOn { get; set; }
    }

    public class ObjectivesInfo
    {
        public const string IDProperty = "_id";
        public const string ValueProperty = "vl";
        public const string MeasurementProperty = "ms";
        public const string StatusProperty = "st";

        [BsonElement(IDProperty)]
        public string ID { get; set; }

        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        [BsonElement(MeasurementProperty)]
        public string Measurement { get; set; }

        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }
}
