using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;

namespace Phytel.Web.Cache
{
    [MongoIndex(Keys = new string[] { KeyProperty })]
    // time to live is set to zero because expiry is updated every time the document is touched so when the item expires it should be removed
    [MongoIndex(Keys = new string[] { ExpiryProperty }, TimeToLive = 0)]
    [MongoCollection(CollectionName = "Cache")]
    public class MongoCacheItem : IMongoEntity<ObjectId>
    {
        public const string IdProperty = "_id";
        [BsonId()]
        public ObjectId Id { get; set; }

        public const string KeyProperty = "k";

        [BsonElement(KeyProperty)]
        public string Key { get; set; }

        public const string ValueProperty = "v";

        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        public const string ExpiryProperty = "e";

        [BsonElement(ExpiryProperty)]
        public DateTime Expiry { get; set; }
    }
}
