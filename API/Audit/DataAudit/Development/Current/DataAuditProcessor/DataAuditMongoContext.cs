using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataAuditProcessor
{
    public class DataAuditMongoContext: MongoContext
    {
        public DataAuditMongoContext(string configName, string dbName)
            : base(configName, dbName, true, "Audit")
        {

        }
    }
}
