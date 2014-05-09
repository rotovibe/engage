using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.DataDomain.PatientObservation.MongoDB.DTO
{
    public class ObservationValue
    {
        public ObservationValue() { }

        public const string ValueIdProperty = "vid";
        [BsonElement(ValueIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId ValueId { get; set; }

        public const string ValueProperty = "val";
        [BsonElement(ValueProperty)]
        [BsonIgnoreIfNull(true)]
        public string Value { get; set; }

        public const string TextProperty = "txt";
        [BsonElement(TextProperty)]
        [BsonIgnoreIfNull(true)]
        public string Text { get; set; }
    }
}
