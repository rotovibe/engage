using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Phytel.API.Interface
{
    public interface IMEDataSource
    {        
        [BsonElement("extrid")]
        [BsonIgnoreIfNull(true)]
        string ExternalRecordId { get; set; }
        
        [BsonElement("dsrc")]
        [BsonIgnoreIfNull(true)]
        string DataSource { get; set; }
    }
}
