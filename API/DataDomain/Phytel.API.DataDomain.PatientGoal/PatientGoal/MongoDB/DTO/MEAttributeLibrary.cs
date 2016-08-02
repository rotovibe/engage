using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Common;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System;
using System.Collections.Generic;


namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { TypeProperty, DeleteFlagProperty }, Unique = false)]
    public class MEAttributeLibrary : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEAttributeLibrary(string userId)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = DateTime.UtcNow;
        }

        public const string IdProperty = "_id";
        public const string NameProperty = "nm";
        public const string TypeProperty = "type";
        public const string ControlTypeProperty = "ctype";
        public const string OptionsProperty = "opt";
        public const string OrderProperty = "o";
        public const string RequiredProperty = "req";

        #region Standard IMongoEntity Constants
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";
        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedOnProperty = "rcon";
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
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        [BsonElement(UpdatedByProperty)]
        public ObjectId? UpdatedBy { get; set; }

        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime? TTLDate { get; set; }

        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime? LastUpdatedOn { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; private set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; private set; }

        #endregion
    }

    public enum EntityType
    {
        Goal = 1,
        Task = 2,
    }

    public enum AttributeControlType
    {
        Single = 1,
        Multi = 2,
        Date = 3,
        DateTime = 4,
        Text = 5
    }
}

