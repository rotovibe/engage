using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Common;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Phytel.API.DataDomain.PatientGoal.MongoDB.DTO
{
    public class MAttribute
    {
        public MAttribute() {}

        public const string NameProperty = "nm";
        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }

        public const string ValueProperty = "val";
        [BsonElement(ValueProperty)]
        [BsonIgnoreIfNull(true)]
        public string Value { get; set; }

        public const string ControlTypeProperty = "aty";
        [BsonElement(ControlTypeProperty)]
        [BsonIgnoreIfNull(false)]
        public AttributeControlType ControlType { get; set; }

        public const string OrderProperty = "o";
        [BsonElement(OrderProperty)]
        [BsonIgnoreIfNull(true)]
        public int Order { get; set; }
    }
}

