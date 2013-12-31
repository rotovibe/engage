using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEProgram : MEProgramBase, IMongoEntity<ObjectId>
    {
        public MEProgram() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string TemplateNameProperty = "ptn";
        [BsonElement(TemplateNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string TemplateName { get; set; }

        public const string TemplateStatusProperty = "tempstatus";
        [BsonElement(TemplateStatusProperty)]
        [BsonIgnoreIfNull(true)]
        public string TemplateStatus { get; set; }

        public const string TTLDateProperty = "ttl";
        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? TTLDate { get; set; }
    }
}
