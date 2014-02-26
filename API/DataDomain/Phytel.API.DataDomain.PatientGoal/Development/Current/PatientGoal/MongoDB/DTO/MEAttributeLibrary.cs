using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Common;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System.Collections.Generic;


namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { TypeProperty, DeleteFlagProperty }, Unique = false)]
    public class MEAttributeLibrary : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEAttributeLibrary() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string NameProperty = "nm";
        public const string TypeProperty = "type";
        public const string ControlTypeProperty = "ctype";
        public const string OptionsProperty = "opt";
        public const string OrderProperty = "o";
        public const string RequiredProperty = "req";

        #region Standard IMongoEntity Constants
        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";
        #endregion

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }

        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(true)]
        public EntityType Type { get; set; }
        
        [BsonElement(ControlTypeProperty)]
        [BsonIgnoreIfNull(false)]
        public AttributeControlType ControlType { get; set; }

        [BsonElement(OptionsProperty)]
        [BsonIgnoreIfNull(true)]
        public Dictionary<int, string> Options { get; set; }

        [BsonElement(OrderProperty)]
        [BsonIgnoreIfNull(true)]
        public int Order { get; set; }

        [BsonDefaultValue(false)]
        [BsonElement(RequiredProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Required { get; set; }

        #region Standard IMongoEntity Implementation
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
        public System.DateTime? TTLDate { get; set; }

        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public System.DateTime? LastUpdatedOn { get; set; }
        #endregion
    }

    public enum EntityType
    {
        Goal = 1,
        Task = 2,
    }
}

