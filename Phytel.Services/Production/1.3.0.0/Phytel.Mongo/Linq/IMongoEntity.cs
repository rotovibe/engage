using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.Mongo.Linq
{
    /// <summary>
    /// Interface that all MongoSet<typeparamref name="T"/> types must implement
    /// </summary>
    public interface IMongoEntity<TKey>
    {
        TKey Id { get; }
    }
}
