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
    public class Action : IMongoEntity<ObjectId>, IMEEntity
    {
        public Action() { }

        public const string IdProperty = "_id";
        [BsonElement(IdProperty)]
        public ObjectId Id { get; set; }

        public const string ModuleIdProperty = "mid";
        [BsonElement(ModuleIdProperty)]
        public ObjectId ModuleId { get; set; }

        public const string NameProperty = "nm";
        [BsonElement(NameProperty)]
        public string Name { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        public const string ObjectivesProperty = "obj";
        [BsonElement(ObjectivesProperty)]
        public List<Objective> Objectives { get; set; }

        public const string StepsProperty = "sps";
        [BsonElement(StepsProperty)]
        public List<Step> Steps { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }

        public BsonDocument ExtraElements
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double Version
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ObjectId? UpdatedBy
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool DeleteFlag
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime? TTLDate
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime? LastUpdatedOn
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ObjectId RecordCreatedBy
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime RecordCreatedOn
        {
            get { throw new NotImplementedException(); }
        }
    }
}
