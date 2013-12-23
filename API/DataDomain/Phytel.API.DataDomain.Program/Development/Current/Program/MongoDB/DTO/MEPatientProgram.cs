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
    public class MEPatientProgram : MEProgramBase, IMongoEntity<ObjectId>
    {
        public MEPatientProgram() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string PatientIdProperty = "pid";
        public const string ContractProgramIdProperty = "cpid";
        public const string ProgramStateProperty = "stat";
        public const string TTLDateProperty = "ttl";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId PatientId { get; set; }

        [BsonElement(ContractProgramIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId ContractProgramId { get; set; }

        [BsonElement(ProgramStateProperty)]
        public ProgramState ProgramState { get; set; }

        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? TTLDate { get; set; }
    }
}
