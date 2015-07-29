using Phytel.Services.Versions;
using System.Collections.Generic;

namespace Phytel.Services.Migrations
{
    public interface IMigrationDatabaseLogRepository
    {
        List<MigrationLog> DeleteVersionsLessThan(string product, SemanticVersion version, Database db);

        List<MigrationLog> Delete(List<MigrationLog> migrationDatabaseLogs);

        MigrationLog Delete(MigrationLog migrationDatabaseLog);

        MigrationLog Delete(string product, SemanticVersion version, Database db, Error err = null);

        MigrationLog GetLatestVersion(string product, Database db);

        List<MigrationLog> GetLatestVersions(string product, List<Database> dbs);

        MigrationLog Save(string product, SemanticVersion version, Database db, Error err = null);
    }
}