using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.Contact.MongoDB.DTO
{
    public class MEPhone
    {
        public const string IDProperty = "_id";
        public const string NumberProperty = "num";
        public const string TypeIdProperty = "typeid";
        public const string PreferredProperty = "pf";
        public const string OptOutProperty = "oo";
        public const string DeleteFlagProperty = "del";

        [BsonElement(IDProperty)]
        public ObjectId Id { get; set; }

        [BsonElement(NumberProperty)]
        public string Number { get; set; }

        [BsonElement(TypeIdProperty)]
        public ObjectId TypeId { get; set; }

        [BsonElement(PreferredProperty)]
        public bool Preferred { get; set; }

        [BsonElement(OptOutProperty)]
        public bool OptOut { get; set; }

        [BsonElement(DeleteFlagProperty)]
        public bool DeleteFlag { get; set; }
    }
}
