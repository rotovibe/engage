using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class Language
    {
        public const string LanguageIdProperty = "lid";
        public const string PreferredProperty = "pf";

        [BsonElement(LanguageIdProperty)]
        public ObjectId LookUpLanguageId { get; set; }

        [BsonElement(PreferredProperty)]
        public bool Preferred { get; set; }
    }
}
