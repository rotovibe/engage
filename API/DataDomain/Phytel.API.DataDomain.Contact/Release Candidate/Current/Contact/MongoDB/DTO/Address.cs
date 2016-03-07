using System.CodeDom;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class Address:IMEDataSource
    {
        public const string IDProperty = "_id";
        public const string TypeIdProperty = "typeid";
        public const string Line1Property = "l1";
        public const string Line2Property = "l2";
        public const string Line3Property = "l3";
        public const string CityProperty = "ci";
        public const string StateIdProperty = "stid";
        public const string PostalCodeProperty = "pc";
        public const string PreferredProperty = "pf";
        public const string OptOutProperty = "oo";
        public const string DeleteFlagProperty = "del";
        
        [BsonElement(IDProperty)]
        public ObjectId Id { get; set; }

        [BsonElement(TypeIdProperty)]
        public ObjectId TypeId { get; set; }

        [BsonElement(Line1Property)]
        public string Line1 { get; set; }

        [BsonElement(Line2Property)]
        public string Line2 { get; set; }

        [BsonElement(Line3Property)]
        public string Line3 { get; set; }

        [BsonElement(CityProperty)]
        public string City { get; set; }

        [BsonElement(StateIdProperty)]
        public ObjectId StateId { get; set; }

        [BsonElement(PostalCodeProperty)]
        public string PostalCode { get; set; }

        [BsonElement(PreferredProperty)]
        public bool Preferred { get; set; }

        [BsonElement(OptOutProperty)]
        public bool OptOut { get; set; }

        [BsonElement(DeleteFlagProperty)]
        public bool DeleteFlag { get; set; }

        public const string ExternalRecordIdProperty = "extrid";
        [BsonElement(ExternalRecordIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ExternalRecordId { get; set; }

        public const string DataSourceProperty = "dsrc";
        [BsonElement(DataSourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string DataSource { get; set; }
    }
}
