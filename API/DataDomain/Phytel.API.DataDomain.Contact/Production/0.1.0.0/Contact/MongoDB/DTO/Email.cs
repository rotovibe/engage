﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class Email
    {
        public const string IDProperty = "_id";
        public const string TextProperty = "txt";
        public const string TypeIdProperty = "typeid";
        public const string PreferredProperty = "pf";
        public const string OptOutProperty = "oo";
        public const string DeleteFlagProperty = "del";

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
    }
}
