using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.Services.Mongo.Linq
{
    /// <summary>
    /// Interface that all MongoSet<typeparamref name="T"/> types must implement
    /// </summary>
    public interface IMongoEntity<TKey>
    {
        TKey Id { get; }

        [BsonExtraElements]
        BsonDocument ExtraElements { get; set; }

        [BsonElement("v")]
        [BsonDefaultValue(1.0)]
        double Version { get; set; }

        [BsonElement("ttl")]
        [BsonDefaultValue(null)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        DateTime? TTLDate { get; set; }
    }
}
