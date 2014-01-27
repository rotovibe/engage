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
    public class MEObjective : IMongoEntity<ObjectId>
    {
        public MEObjective() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string ValueProperty = "val";
        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        public const string UnitProperty = "unit";
        [BsonElement(UnitProperty)]
        public string Unit { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }
}
