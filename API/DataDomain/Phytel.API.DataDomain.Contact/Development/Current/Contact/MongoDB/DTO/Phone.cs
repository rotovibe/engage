using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class Phone
    {
        public const string IDProperty = "_id";
        public const string NumberProperty = "num";
        public const string TypeIdProperty = "typeid";
        public const string IsTextProperty = "istxt";
        public const string PhonePreferredProperty = "pfph";
        public const string TextPreferredProperty = "pftxt";
        public const string OptOutProperty = "oo";
        public const string DeleteFlagProperty = "del";

        [BsonElement(IDProperty)]
        public ObjectId Id { get; set; }

        [BsonElement(NumberProperty)]
        public long Number { get; set; }

        [BsonElement(TypeIdProperty)]
        public ObjectId TypeId { get; set; }

        [BsonElement(IsTextProperty)]
        public bool IsText { get; set; }

        [BsonElement(PhonePreferredProperty)]
        public bool PreferredPhone { get; set; }

        [BsonElement(TextPreferredProperty)]
        public bool PreferredText { get; set; }

        [BsonElement(OptOutProperty)]
        public bool OptOut { get; set; }

        [BsonElement(DeleteFlagProperty)]
        public bool DeleteFlag { get; set; }
    }
}
