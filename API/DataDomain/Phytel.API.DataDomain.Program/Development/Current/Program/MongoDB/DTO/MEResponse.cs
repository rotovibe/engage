using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    public class MEResponse : IMongoEntity<ObjectId>
    {
        public MEResponse() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        public const string TextProperty = "txt";
        public const string RequiredProperty = "req";

        [BsonId]
        public ObjectId Id { get; set; }

        public const string OrderProperty = "o";
        [BsonElement(OrderProperty)]
        public int Order { get; set; }

        [BsonElement(TextProperty)]
        public string Text { get; set; }

        public const string StepIdProperty = "sid";
        [BsonElement(StepIdProperty)]
        public ObjectId StepId { get; set; }

        public const string ValueProperty = "val";
        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        public const string NominalProperty = "nml";
        [BsonElement(NominalProperty)]
        public bool Nominal { get; set; }

        [BsonElement(RequiredProperty)]
        public bool Required { get; set; }

        public const string NextStepIdProperty = "nsid";
        [BsonElement(NextStepIdProperty)]
        public ObjectId NextStepId { get; set; }

        public const string SpawnElementProperty = "spwn";
        [BsonElement(SpawnElementProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MESpawnElement> Spawn { get; set; }
    }
}
