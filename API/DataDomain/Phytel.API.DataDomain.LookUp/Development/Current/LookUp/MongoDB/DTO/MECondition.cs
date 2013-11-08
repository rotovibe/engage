using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [BsonIgnoreExtraElements(true)]
    [MongoIndex(Keys = new string[] { IdProperty })]
    public class MECondition : IMongoEntity<ObjectId>
    {
        public MECondition() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string DisplayNameProperty = "dn";
        public const string IsActiveProperty = "isac";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(DisplayNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string DisplayName { get; set; }

        [BsonElement(IsActiveProperty)]
        [BsonIgnoreIfNull(true)]
        public bool IsActive { get; set; }
    }
}
