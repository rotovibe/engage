using Phytel.Services.Versions;
using System;

namespace Phytel.Services.Migrations
{
    public class MigrationLog
    {
        public DateTime RecordCreatedOn { get; set; }

        public string Id { get; set; }

        public SemanticVersion MigrationVersion { get; set; }

        public string Product { get; set; }

        public double Version { get; set; }
    }
}