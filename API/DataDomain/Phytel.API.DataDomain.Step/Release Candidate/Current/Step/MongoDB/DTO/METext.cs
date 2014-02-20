using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Step.DTO
{
    [BsonIgnoreExtraElements(false)]
    public class METext : MEStepBase
    {
        public const string TitleProperty = "t";
        [BsonElement(TitleProperty)]
        [BsonIgnoreIfNull(true)]
        public string Title { get; set; }

        [BsonElement(DescriptionProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonRequired]
        public string Description { get; set; }

        public const string TextPromptProperty = "text";
        [BsonElement(TextPromptProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonRequired]
        public string TextPrompt { get; set; }
    }
}
