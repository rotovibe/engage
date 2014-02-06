using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { SearchFieldsProperty + "." + SearchField.FieldNameProperty, SearchFieldsProperty + "." + SearchField.ValueProperty, SearchFieldsProperty + "." + SearchField.ActiveProperty })]
    public class MECohortPatientView : IMongoEntity<ObjectId>
    {
        public MECohortPatientView() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string PatientIDProperty = "pid";
        public const string SearchFieldsProperty = "sf";
        public const string LastNameProperty = "ln";
        public const string VersionProperty = "v";
        public const string ExtraElementsProperty = "ex";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(PatientIDProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId PatientID { get; set; }

        [BsonElement(SearchFieldsProperty)]
        public List<SearchField> SearchFields { get; set; }

        [BsonElement(LastNameProperty)]
        public string LastName { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue("v1")]
        public string Version { get; set; }

        [BsonExtraElements]
        [BsonIgnoreIfNull(true)]
        [BsonElement(ExtraElementsProperty)]
        Dictionary<string, object> ExtraElements { get; set; }
    }
}

