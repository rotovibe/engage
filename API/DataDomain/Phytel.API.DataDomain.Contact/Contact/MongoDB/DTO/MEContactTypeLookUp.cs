using System;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.Services.Mongo.Linq;

namespace Phytel.API.DataDomain.Contact.MongoDB.DTO
{
    [BsonIgnoreExtraElements()]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEContactTypeLookup : IMongoEntity<ObjectId>
    {
        public MEContactTypeLookup(string userId, DateTime? createdOn)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = createdOn == null || createdOn.Equals(new DateTime()) ? DateTime.UtcNow : (DateTime)createdOn;
        }


        [BsonId]
        [BsonElement(IdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId Id { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(NameProperty)]
        public string Name { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RoleProperty)]
        public string Role { get; set; }

        [BsonElement(ParentIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? ParentId { get; set; }

        [BsonElement(GroupProperty)]
        [BsonIgnoreIfNull(true)]
        public ContactLookUpGroupType GroupId { get; set; }

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        [BsonElement(LastUpdatedByProperty)]
        public ObjectId? UpdatedBy { get; set; }

        [BsonElement(ActiveProperty)]
        public bool Active { get; set; }

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

        public const string IdProperty = "_id";
        
        public const string NameProperty = "nm";
        public const string RoleProperty = "r";
        public const string ParentIdProperty = "ptid";
        public const string TTLDateProperty = "ttl";
        public const string GroupProperty = "g";

        #region Standard IMongoEntity Constants
        public const string VersionProperty = "v";
        public const string DeleteFlagProperty = "del";
        public const string LastUpdatedOnProperty = "uon";
        public const string LastUpdatedByProperty = "uby";
        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedOnProperty = "rcon";
        public const string ActiveProperty = "act";

        #endregion
    }
}
