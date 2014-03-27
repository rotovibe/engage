using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataAuditProcessor
{
    public class DataAuditMongoContext: MongoContext
    {

        public DataAuditMongoContext(string dbName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), dbName, true, "Audit")
        {

        }
    }
}
