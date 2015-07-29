using Phytel.Services.Versions;
using System;

namespace Phytel.Services.Migrations
{
    public class MigrationLog
    {
        public Database Database { get; set; }

        public Error Error { get; set; }

        public string Id { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public SemanticVersion MigrationVersion { get; set; }

        public string Product { get; set; }

        public DateTime RecordCreatedOn { get; set; }
    }
}