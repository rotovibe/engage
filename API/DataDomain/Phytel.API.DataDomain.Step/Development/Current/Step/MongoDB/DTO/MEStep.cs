using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;

namespace Phytel.API.DataDomain.Step.DTO
{
    [BsonIgnoreExtraElements(false)]
    //[MongoIndex(Keys = new string[] { "SOME PROPERTY NAME HERE" })]
    public class MEStep : IMongoEntity<ObjectId> //, ISupportInitialize
    {
        public MEStep() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";

        [BsonId]
        public ObjectId Id { get; set; }

    }
}
