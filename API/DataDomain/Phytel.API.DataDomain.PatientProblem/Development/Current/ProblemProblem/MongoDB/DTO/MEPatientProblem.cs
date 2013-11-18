using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System.Collections.Generic;
using System;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    public class MEPatientProblem : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatientProblem() { Id = ObjectId.GenerateNewId(); }                                                                                                                                                                                                           

        public const string IdProperty = "_id";
        public const string PatientIDProperty = "pid";
        public const string ProblemIDProperty = "prbid";
        public const string ActiveProperty = "a";
        public const string FeaturedProperty = "f";
        public const string LevelProperty = "l";
        public const string StartDateProperty = "sd";
        public const string EndDateProperty = "ed";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(PatientIDProperty)]
        [BsonIgnoreIfNull(true)]
        public string PatientID { get; set; }

        [BsonElement(ProblemIDProperty)]
        [BsonIgnoreIfNull(true)]
        public string ProblemID { get; set; }

        [BsonElement(ActiveProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Active { get; set; }

        [BsonElement(FeaturedProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Featured { get; set; }

        [BsonElement(LevelProperty)]
        [BsonIgnoreIfNull(true)]
        public int Level { get; set; }

        [BsonElement(StartDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime StartDate { get; set; }

        [BsonElement(EndDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime EndDate { get; set; }

        public Dictionary<string, object> ExtraElements { get; set; }
       
        public string Version { get; set; } 

        public string UpdatedBy { get; set; }

        public bool DeleteFlag { get; set; }

        public DateTime TTLDate { get; set; }

        public DateTime LastUpdatedOn { get; set; }

    }
}
