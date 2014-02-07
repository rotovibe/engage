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
    public class MEResponseBase : IMEEntity
    {
        public MEResponseBase() {}

        public const string OrderProperty = "o";
        [BsonElement(OrderProperty)]
        public int Order { get; set; }

        public const string TextProperty = "txt";
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

        public const string RequiredProperty = "req";
        [BsonElement(RequiredProperty)]
        public bool Required { get; set; }

        public const string NextStepIdProperty = "nsid";
        [BsonElement(NextStepIdProperty)]
        public ObjectId NextStepId { get; set; }

        public const string SpawnElementProperty = "spwn";
        [BsonElement(SpawnElementProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MESpawnElement> Spawn { get; set; }

        public const string ExtraElementsProperty = "ex";
        [BsonExtraElements]
        [BsonIgnoreIfNull(true)]
        [BsonElement(ExtraElementsProperty)]
        public Dictionary<string, object> ExtraElements { get; set; }

        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue("v1")]
        public string Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
        [BsonIgnoreIfNull(true)]
        public string UpdatedBy { get; set; }

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
    }
}
