using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEPatientIntervention : MEGoalBase, IMongoEntity<ObjectId>
    {
        public MEPatientIntervention() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string CategoryProperty = "cat";
        [BsonElement(CategoryProperty)]
        [BsonIgnoreIfNull(false)]
        public string Category { get; set; }

        public const string AssignedToProperty = "ato";
        [BsonElement(AssignedToProperty)]
        [BsonIgnoreIfNull(false)]
        public string AssignedTo { get; set; }

        public const string BarriersProperty = "bar";
        [BsonElement(BarriersProperty)]
        [BsonIgnoreIfNull(false)]
        public List<ObjectId> Barriers { get; set; }
    }
}
