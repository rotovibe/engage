using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    public class MEPatientProgramResponse : ResponseBase, IMongoEntity<ObjectId>
    {
        public MEPatientProgramResponse() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
