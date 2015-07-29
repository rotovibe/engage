using Phytel.Services.Versions;
using System.Collections.Generic;

namespace Phytel.Services.Migrations
{
    public interface IMigrationHandler
    {
        void Migrate(string product, SemanticVersion versionTarget, List<Connection> connectionOverrides);

        void Migrate(string product, int major, int minor, int patch, List<Connection> connectionOverrides);

        List<MigrationLog> GetLatestVersion(string product, List<Connection> connectionOverrides);
    }
}