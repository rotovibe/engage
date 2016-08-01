using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class Email:IMEDataSource
    {
        public const string IDProperty = "_id";
        public const string TextProperty = "txt";
        public const string TypeIdProperty = "typeid";
        public const string PreferredProperty = "pf";
        public const string OptOutProperty = "oo";
        public const string DeleteFlagProperty = "del";
        public const string DataSourceProperty = "dsrc";

        [BsonElement(IDProperty)]
        public ObjectId Id { get; set; }

        [BsonElement(TextProperty)]
        public string Text { get; set; }

        [BsonElement(TypeIdProperty)]
        public ObjectId TypeId { get; set; }
        
        [BsonElement(PreferredProperty)]
        public bool Preferred { get; set; }

        [BsonElement(OptOutProperty)]
        public bool OptOut { get; set; }

        [BsonElement(DeleteFlagProperty)]
        public bool DeleteFlag { get; set; }
        
        [BsonElement(DataSourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string DataSource { get; set; }

        public const string ExternalRecordIdProperty = "extrid";
        [BsonElement(ExternalRecordIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ExternalRecordId { get; set; }
    }
}
