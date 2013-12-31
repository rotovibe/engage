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
    public class MEContractProgram : MEProgramBase, IMongoEntity<ObjectId>
    {
        public MEContractProgram(){ Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string ProgramTemplateIdProperty = "ptid";
        [BsonElement(ProgramTemplateIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId ProgramTemplateId { get; set; }

        public const string ContractIdProperty = "cid";
        [BsonElement(ContractIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ContractId { get; set; }

        public const string TTLDateProperty = "ttl";
        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? TTLDate { get; set; }
    }
}
