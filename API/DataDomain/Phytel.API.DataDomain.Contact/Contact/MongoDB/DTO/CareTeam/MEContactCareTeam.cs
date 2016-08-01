using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;

namespace Phytel.API.DataDomain.Contact.MongoDB.DTO
{
    [BsonIgnoreExtraElements()]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { ContactIdProperty }, TimeToLive = 0, Unique = true)]
    public class MEContactCareTeam : IMongoEntity<ObjectId>
    {
        public MEContactCareTeam(string userId, DateTime? createdOn)
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

        [BsonElement(ContactIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId ContactId { get; set; }

        [BsonElement(MembersProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MECareTeamMember> MeCareTeamMembers { get; set; }

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(LastUpdatedByProperty)]
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

        public const string IdProperty = "_id";
        public const string TTLDateProperty = "ttl";
        public const string ContactIdProperty = "cid";
        public const string MembersProperty = "mbrs";

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
