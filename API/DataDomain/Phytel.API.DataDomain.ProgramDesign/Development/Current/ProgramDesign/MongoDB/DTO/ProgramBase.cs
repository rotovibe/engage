using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO
{
    public class ProgramBase : PlanElement
    {
        public ProgramBase() {}

        public const string EndDateProperty = "ed";
        [BsonElement(EndDateProperty)]
        [BsonIgnoreIfNull(false)]
        public DateTime? EndDate { get; set; }

        public const string NameProperty = "nm";
        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }

        public const string ShortNameProperty = "sn";
        [BsonElement(ShortNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string ShortName { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        [BsonIgnoreIfNull(true)]
        public string Description { get; set; }

        public const string ClientProperty = "cli";
        [BsonElement(ClientProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? Client { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status Status { get; set; }

        public const string ObjectivesInfoProperty = "obj";
        [BsonElement(ObjectivesInfoProperty)]
        [BsonIgnoreIfNull(true)]
        public List<Objective> Objectives { get; set; }

        public const string ModulesProperty = "ms";
        [BsonElement(ModulesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MEModule> Modules { get; set; }
    }
}
