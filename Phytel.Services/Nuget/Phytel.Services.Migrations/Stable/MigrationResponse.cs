using Phytel.Services.Versions;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Services.Migrations
{
    public class MigrationResponse
    {
        public MigrationLog LatestMigration { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}
