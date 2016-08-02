using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class Phone:IMEDataSource
    {
        public const string IDProperty = "_id";
        [BsonElement(IDProperty)]
        public ObjectId Id { get; set; }

        public const string NumberProperty = "num";
        [BsonElement(NumberProperty)]
        public long Number { get; set; }

        public const string ExtNumberProperty = "extn";
        [BsonElement(ExtNumberProperty)]
        [BsonIgnoreIfNull]
        public string ExtNumber { get; set; }

        public const string TypeIdProperty = "typeid";
        [BsonElement(TypeIdProperty)]
        public ObjectId TypeId { get; set; }

        public const string IsTextProperty = "istxt";
        [BsonElement(IsTextProperty)]
        public bool IsText { get; set; }

        public const string PhonePreferredProperty = "pfph";
        [BsonElement(PhonePreferredProperty)]
        public bool PreferredPhone { get; set; }

        public const string TextPreferredProperty = "pftxt";
        [BsonElement(TextPreferredProperty)]
        public bool PreferredText { get; set; }

        public const string OptOutProperty = "oo";
        [BsonElement(OptOutProperty)]
        public bool OptOut { get; set; }

        public const string DataSourceProperty = "dsrc";
        [BsonElement(DataSourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string DataSource { get; set; }

        public const string DeleteFlagProperty = "del";
        [BsonElement(DeleteFlagProperty)]
        public bool DeleteFlag { get; set; }

        public const string ExternalRecordIdProperty = "extrid";
        [BsonElement(ExternalRecordIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ExternalRecordId { get; set; }
    }
}
