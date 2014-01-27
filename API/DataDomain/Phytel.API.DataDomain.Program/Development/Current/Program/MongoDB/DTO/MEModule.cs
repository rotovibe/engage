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
    public class MEModules : MEPlanElement, IMongoEntity<ObjectId>
    {
        public MEModules() { Id = ObjectId.GenerateNewId(); }

        public const string IDProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string ProgramIdProperty = "progid";
        [BsonElement(ProgramIdProperty)]
        public ObjectId ProgramId { get; set; }

        public const string NameProperty = "nm";
        [BsonElement(NameProperty)]
        public string Name { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        public const string ObjectivesProperty = "obj";
        [BsonElement(ObjectivesProperty)]
        public List<MEObjective> Objectives { get; set; }

        public const string ActionsProperty = "acts";
        [BsonElement(ActionsProperty)]
        public List<MEAction> Actions { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }
}
