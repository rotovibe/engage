using Phytel.Services.Versions;
using System.Reflection;

namespace Phytel.Services.Migrations
{
    public interface IMigrationHost
    {
        void MigrateToVersion(string product, SemanticVersion versionNew);

        void MigrateToVersion(string product, int major, int minor, int patch);

        MigrationLog GetLatestMigration(string product);
    }
}