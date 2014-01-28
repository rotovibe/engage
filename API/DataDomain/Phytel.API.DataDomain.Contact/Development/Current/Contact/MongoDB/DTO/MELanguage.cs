using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class MELanguage
    {
        public const string IDProperty = "_id";
        public const string LanguageIdProperty = "lid";
        public const string PreferredProperty = "pf";

        [BsonElement(IDProperty)]
        public ObjectId Id { get; set; }

        [BsonElement(LanguageIdProperty)]
        public ObjectId LookUpLanguageId { get; set; }

        [BsonElement(PreferredProperty)]
        public bool Preferred { get; set; }
    }
}
