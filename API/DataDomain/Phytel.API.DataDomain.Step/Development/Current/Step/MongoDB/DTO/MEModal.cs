using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Step.DTO
{
    [BsonIgnoreExtraElements(false)]
    public class MEModal : MEStepBase
    {
        public const string TitleProperty = "t";
        [BsonElement(TitleProperty)]
        [BsonIgnoreIfNull(true)]
        public string Title { get; set; }

        public const string TextPromptProperty = "txt";
        [BsonElement(TextPromptProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonRequired]
        public string Text { get; set; }

        public const string ResponseProperty = "resp";
        [BsonElement(ResponseProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MEResponse> Response { get; set; }
    }
}
