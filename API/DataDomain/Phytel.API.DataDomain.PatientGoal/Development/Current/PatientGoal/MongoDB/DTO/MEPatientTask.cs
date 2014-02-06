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
    public class MEPatientTask : MEGoalBase, IMongoEntity<ObjectId>
    {
        public MEPatientTask() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }


        public const string TargetValueProperty = "tv";
        [BsonElement(TargetValueProperty)]
        [BsonIgnoreIfNull(false)]
        public string TargetValue { get; set; }

        public const string TargetDateProperty = "td";
        [BsonElement(TargetDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? TargetDate { get; set; }

        public const string BarriersProperty = "bar";
        [BsonElement(BarriersProperty)]
        [BsonIgnoreIfNull(false)]
        public List<ObjectId> Barriers { get; set; }
    }
}
