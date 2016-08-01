using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class CommMode
    {
        public const string ModeIdProperty = "mdid";
        public const string PreferredProperty = "pf";
        public const string OptOutProperty = "oo";
        
        [BsonElement(ModeIdProperty)]
        public ObjectId ModeId { get; set; }

        [BsonElement(PreferredProperty)]
        public bool Preferred { get; set; }

        [BsonElement(OptOutProperty)]
        public bool OptOut { get; set; }
    }
}
