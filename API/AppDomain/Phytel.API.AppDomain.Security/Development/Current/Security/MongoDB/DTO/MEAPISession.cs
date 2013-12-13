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
        public const string UserNameProperty = "un";
        public const string UserIDProperty = "uid";
        public const string ProductProperty = "prd";
        public const string APIKeyProperty = "apiKey";
//        public const string SessionLengthInMinutesProperty = "slim";
        public const string SessionTimeOutProperty = "sto";
        public const string VersionProperty = "v";
        public const string ExtraElementsProperty = "ex";

        [BsonId]
        public ObjectId Id { get; set; } // token

        [BsonElement(UserNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string UserName { get; set; }

        [BsonElement(UserIDProperty)]
        [BsonIgnoreIfNull(true)]
        public string UserID { get; set; }

        [BsonElement(APIKeyProperty)]
        [BsonIgnoreIfNull(true)]
        public string APIKey { get; set; }

        [BsonElement(ProductProperty)]
        [BsonIgnoreIfNull(true)]
        public string Product { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        [BsonElement(SessionTimeOutProperty)]
        public DateTime SessionTimeOut { get; set; }

        //[BsonElement(SessionLengthInMinutesProperty)]
        //[BsonDefaultValue(60)]
        //public int SessionLengthInMinutes { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue("v1")]
        public string Version { get; set; }

        [BsonExtraElements]
        [BsonIgnoreIfNull(true)]
        [BsonElement(ExtraElementsProperty)]
        Dictionary<string, object> ExtraElements { get; set; }
    }
}
