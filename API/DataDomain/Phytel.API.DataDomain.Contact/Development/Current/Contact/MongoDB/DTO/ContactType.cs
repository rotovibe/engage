using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class ContactType
    {
        public const string ContactTypeLookupIdProperty = "lkid";
        [BsonElement(ContactTypeLookupIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ContactTypeLookupId { get; set; }

        public const string ChildrenProperty = "chld";
        [BsonIgnoreIfNull(true)]
        [BsonElement(ChildrenProperty)]
        public List<ContactType> Children { get; set; }
    }
}