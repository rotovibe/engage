using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class MAttribute
    {
        public const string AttributeIdProperty = "aid";
        public const string ValueProperty = "val";

        [BsonElement(AttributeIdProperty)]
        public ObjectId AttributeId { get; set; }

        [BsonElement(ValueProperty)]
        public List<string> Values { get; set; }
    }
}

