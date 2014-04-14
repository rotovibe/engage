using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO
{
    public class Objective
    {
        public Objective() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        [BsonElement(IdProperty)]
        public ObjectId Id { get; set; }

        public const string ValueProperty = "val";
        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        public const string UnitProperty = "u";
        [BsonElement(UnitProperty)]
        public string Units { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }
}
