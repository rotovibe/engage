using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;

namespace Phytel.API.DataDomain.Program.DTO
{
    [BsonIgnoreExtraElements(false)]
    //[MongoIndex(Keys = new string[] { "SOME PROPERTY NAME HERE" })]
    public class MEProgram : IMongoEntity<ObjectId> //, ISupportInitialize
    {
        public MEProgram() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";

        [BsonId]
        public ObjectId Id { get; set; }

    }
}
