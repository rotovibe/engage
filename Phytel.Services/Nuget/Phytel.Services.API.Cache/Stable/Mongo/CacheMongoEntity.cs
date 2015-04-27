using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;
using System;

namespace Phytel.Services.API.Cache.Mongo
{
    [MongoIndex(Keys = new string[] { KeyProperty })]
    // time to live is set to zero because expiry is updated every time the document is touched so when the item expires it should be removed
    [MongoIndex(Keys = new string[] { ExpiryProperty }, TimeToLive = 0)]
    [MongoCollection(CollectionName = "Cache")]
    public class CacheMongoEntity : IMongoEntity<ObjectId>
    {
        public const string IdProperty = "_id";
        public const string KeyProperty = "k";
        public const string ValueProperty = "v";
        public const string ExpiryProperty = "e";

        public const string ExtraElementsProperty = "ex";
        public const string TTLDateProperty = "ttl";
        public const string VersionProperty = "ver";

        public CacheMongoEntity()
        {
        }

        [BsonId()]
        public ObjectId Id { get; set; }

        [BsonElement(KeyProperty)]
        public string Key { get; set; }

        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        [BsonElement(ExpiryProperty)]
        public DateTime Expiry { get; set; }

        [BsonElement(ExtraElementsProperty)]
        public BsonDocument ExtraElements { get; set; }

        [BsonElement(TTLDateProperty)]
        public DateTime? TTLDate { get; set; }

        [BsonElement(VersionProperty)]
        public double Version { get; set; }
    }
}
