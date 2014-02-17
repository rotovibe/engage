using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Common;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class MAttribute
    {
        public MAttribute() {}

        public const string IdProperty = "id";
        [BsonElement(IdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId Id { get; set; }

        public const string ValueProperty = "val";
        [BsonElement(ValueProperty)]
        [BsonIgnoreIfNull(true)]
        public List<string> Values { get; set; }
    }
}

