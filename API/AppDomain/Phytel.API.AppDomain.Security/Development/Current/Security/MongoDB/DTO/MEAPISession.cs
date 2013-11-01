using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System;

namespace Phytel.API.AppDomain.Security
{
    [BsonIgnoreExtraElements(true)]
    [MongoIndex(Keys = new string[] { TTLProperty }, TimeToLive=0)]
    public class MEAPISession : IMongoEntity<ObjectId>
    {
        public MEAPISession() { Id = ObjectId.GenerateNewId(); }

        public const string UserNameProperty = "un";
        public const string ProductProperty = "prd";
        public const string APIKeyProperty = "apiKey";
        public const string SessionTimeOutProperty = "sto";
        public const string TTLProperty = "ttl";

        [BsonId]
        public ObjectId Id { get; set; } // token

        [BsonElement(UserNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string UserName { get; set; }

        [BsonElement(APIKeyProperty)]
        [BsonIgnoreIfNull(true)]
        public string APIKey { get; set; }

        [BsonElement(ProductProperty)]
        [BsonIgnoreIfNull(true)]
        public string Product { get; set; }

        [BsonElement(TTLProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime SessionTimeOut { get; set; }

        [BsonElement(SessionTimeOutProperty)]
        [BsonIgnoreIfNull(true)]
        public int SessionLengthInMinutes { get; set; }

    }
}
