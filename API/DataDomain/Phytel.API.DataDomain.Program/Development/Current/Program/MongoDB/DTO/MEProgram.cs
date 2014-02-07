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
    public class MEProgram : MEProgramBase, IMongoEntity<ObjectId>
    {
        public MEProgram() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string TemplateNameProperty = "tn";
        [BsonElement(TemplateNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string TemplateName { get; set; }
    }
}
