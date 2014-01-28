﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class MECommMode
    {
        public const string IDProperty = "_id";
        public const string ModeIdProperty = "mid";
        public const string PreferredProperty = "pf";
        public const string OptOutProperty = "oo";
        
        [BsonElement(IDProperty)]
        public ObjectId Id { get; set; }

        [BsonElement(ModeIdProperty)]
        public ObjectId ModeId { get; set; }

        [BsonElement(PreferredProperty)]
        public bool Preferred { get; set; }

        [BsonElement(OptOutProperty)]
        public bool OptOut { get; set; }
    }
}
