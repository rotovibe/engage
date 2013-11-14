using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;

namespace Phytel.API.DataDomain.Cohort.DTO
{
    [BsonIgnoreExtraElements(false)]
    //[MongoIndex(Keys = new string[] { "SOME PROPERTY NAME HERE" })]
    public class MECohort : IMongoEntity<ObjectId> //, ISupportInitialize
    {
        public MECohort() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";

        [BsonId]
        public ObjectId Id { get; set; }

    }
}
