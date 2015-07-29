using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.Services.Migrations
{
    public class MigrationsResponse
    {
        public List<MigrationLog> Migrations { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}