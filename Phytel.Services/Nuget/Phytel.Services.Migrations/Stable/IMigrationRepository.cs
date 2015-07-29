using Phytel.Services.Versions;
using System.Collections.Generic;

namespace Phytel.Services.Migrations
{
    public interface IMigrationRepository
    {
        MigrationLog Delete(string product, SemanticVersion version);

        List<MigrationLog> Get(string product = null);

        MigrationLog Save(MigrationLog migrationLog);

        MigrationLog Save(string product, SemanticVersion version);
    }
}