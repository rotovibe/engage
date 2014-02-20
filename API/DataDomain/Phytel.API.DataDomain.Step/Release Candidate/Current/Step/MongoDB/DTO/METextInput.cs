using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Step.DTO
{
    [BsonIgnoreExtraElements(false)]
    public class METextInput : MEStepBase
    {
        public const string InstructionsProperty = "instructions";
        [BsonElement(InstructionsProperty)]
        [BsonIgnoreIfNull(true)]
        public string Instructions { get; set; }

        public const string TextPromptProperty = "text";
        [BsonElement(TextPromptProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonRequired]
        public string TextPrompt { get; set; }

        public const string ResponseProperty = "responses";
        [BsonElement(ResponseProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MEResponse> Response { get; set; }
    }
}
