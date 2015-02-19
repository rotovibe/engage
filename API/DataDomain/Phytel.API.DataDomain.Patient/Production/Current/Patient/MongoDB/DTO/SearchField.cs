using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class SearchField
    {
        public const string FieldNameProperty = "fldn";
        public const string ValueProperty = "val";
        public const string ActiveProperty = "act";

        public SearchField()
        {
            Value = null;
        }

        [BsonElement(FieldNameProperty)]
        public string FieldName { get; set; }

        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        [BsonElement(ActiveProperty)]
        public bool Active { get; set; }
    }
}

