using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Step.DTO
{
    [BsonIgnoreExtraElements(false)]
    public class MEDateTime : MEStepBase
    {
        public const string InstructionsProperty = "inst";
        [BsonElement(InstructionsProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonRequired]
        public string Instructions { get; set; }

        [BsonElement(QuestionProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonRequired]
        public string Question { get; set; }

        public const string ResponseProperty = "resp";
        [BsonElement(ResponseProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MEResponse> Response { get; set; }

        public const string IncludeTimeProperty = "it";
        [BsonElement(IncludeTimeProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonRequired]
        public bool IncludeTime { get; set; }
    }
}
