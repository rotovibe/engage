using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.Module.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEModule : IMongoEntity<ObjectId>, IMEEntity //, ISupportInitialize
    {
        public MEModule() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string NameProperty = "nm";
        public const string ObjectiveProperty = "obj";
        public const string DescriptionProperty = "dsc";
        public const string StatusProperty = "stat";

        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";

        [BsonId]
        public ObjectId Id { get; set; }


        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }

        [BsonElement(ObjectiveProperty)]
        public List<Objective> Objectives { get; set; }

        [BsonElement(DescriptionProperty)]
        [BsonIgnoreIfNull(true)]
        public string Description { get; set; }

        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public string Status { get; set; }



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

    public class Objective
    {
        public const string IDProperty = "_id";

        [BsonElement(IDProperty)]
        public string ID { get; set; }
    }
}
