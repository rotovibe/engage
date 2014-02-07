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
    public class MEContractProgram : MEProgramBase, IMongoEntity<ObjectId>
    {
        public MEContractProgram(){ Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string ProgramTemplateIdProperty = "ptid";
        [BsonElement(ProgramTemplateIdProperty)]
        [BsonIgnoreIfNull(false)]
        public ObjectId ProgramTemplateId { get; set; }

        public const string PopulationProperty = "pop";
        [BsonElement(PopulationProperty)]
        [BsonIgnoreIfNull(false)]
        public string Population { get; set; }

        public const string ContractIdProperty = "cid";
        [BsonElement(ContractIdProperty)]
        [BsonIgnoreIfNull(false)]
        public string ContractId { get; set; }
    }
}
