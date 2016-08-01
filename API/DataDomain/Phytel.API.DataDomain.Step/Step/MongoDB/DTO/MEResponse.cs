using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Step.DTO
{
    [BsonIgnoreExtraElements(false)]
    public class MEResponse
    {
        public MEResponse() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string OrderProperty = "order";
        [BsonElement(OrderProperty)]
        [BsonIgnoreIfNull(true)]
        public int Order { get; set; }

        public const string TextProperty = "text";
        [BsonElement(TextProperty)]
        [BsonIgnoreIfNull(true)]
        public string Text { get; set; }

        public const string InputTextProperty = "inputtext";
        [BsonElement(InputTextProperty)]
        [BsonIgnoreIfNull(true)]
        public string InputText { get; set; }

        public const string StepIdProperty = "stepid";
        [BsonElement(StepIdProperty)]
        public ObjectId StepId { get; set; }

        public const string NominalProperty = "nml";
        [BsonElement(NominalProperty)]
        public bool Nominal { get; set; }

        public const string ResponsePathIdProperty = "responsepathid";
        [BsonElement(ResponsePathIdProperty)]
        public ObjectId ResponsePathId { get; set; }
    }
}
