using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.Security
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { SessionTimeOutProperty }, TimeToLive = 0)]
    public class MEAPISession : IMongoEntity<ObjectId>
    {
        public MEAPISession() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string SecurityTokenProperty = "stkn";
        public const string UserNameProperty = "un";
        public const string UserIdProperty = "uid";
        public const string SQLUserIDProperty = "suid";
        public const string ContractNumberProperty = "cnum";
        public const string ProductProperty = "prd";
        public const string APIKeyProperty = "apiKey";
        public const string SessionLengthInMinutesProperty = "slim";
        public const string SessionTimeOutProperty = "sto";
        public const string VersionProperty = "v";

        [BsonId]
        public ObjectId Id { get; set; } // token

        [BsonElement(SecurityTokenProperty)]
        [BsonIgnoreIfNull(true)]
        public string SecurityToken { get; set; }

        [BsonElement(UserNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string UserName { get; set; }

        [BsonElement(UserIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId UserId { get; set; }

        [BsonElement(SQLUserIDProperty)]
        [BsonIgnoreIfNull(true)]
        public string SQLUserId { get; set; }

        [BsonElement(APIKeyProperty)]
        [BsonIgnoreIfNull(true)]
        public string APIKey { get; set; }

        [BsonElement(ContractNumberProperty)]
        [BsonIgnoreIfNull(true)]
        public string ContractNumber { get; set; }

        [BsonElement(ProductProperty)]
        [BsonIgnoreIfNull(true)]
        public string Product { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement(SessionTimeOutProperty)]
        public DateTime SessionTimeOut { get; set; }

        [BsonElement(SessionLengthInMinutesProperty)]
        [BsonDefaultValue(60)]
        public int SessionLengthInMinutes { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }
    }
}
