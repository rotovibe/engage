using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    public class ResponseBase
    {
        public ResponseBase() {}

        public const string OrderProperty = "o";
        [BsonElement(OrderProperty)]
        public int Order { get; set; }

        public const string TextProperty = "txt";
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

        public const string RequiredProperty = "req";
        [BsonElement(RequiredProperty)]
        public bool Required { get; set; }

        public const string NextStepIdProperty = "nsid";
        [BsonElement(NextStepIdProperty)]
        public ObjectId NextStepId { get; set; }

        public const string SpawnElementProperty = "spwn";
        [BsonElement(SpawnElementProperty)]
        [BsonIgnoreIfNull(true)]
        public List<SpawnElement> Spawn { get; set; }

        #region // new properties for reporting
        public const string SelectedProperty = "sel";
        [BsonElement(SelectedProperty)]
        public bool Selected { get; set; }

        public const string StepSourceIdProperty = "ssid";
        [BsonElement(StepSourceIdProperty)]
        public ObjectId StepSourceId { get; set; }
        #endregion
    }
}
