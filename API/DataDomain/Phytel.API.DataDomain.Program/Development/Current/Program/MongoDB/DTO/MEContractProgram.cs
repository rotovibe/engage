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

        public const string ProgramTemplateIdProperty = "ptid";
        public const string ContractIdProperty = "cid";
        public const string TTLDateProperty = "ttl";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(ProgramTemplateIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId ProgramTemplateId { get; set; }

        [BsonElement(ContractIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId ContractId { get; set; }

        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? TTLDate { get; set; }
    }
}
