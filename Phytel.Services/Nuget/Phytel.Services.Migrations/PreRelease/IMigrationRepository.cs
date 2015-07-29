using Phytel.Services.Versions;
using System;
using System.Collections.Generic;

namespace Phytel.Services.Migrations
{
    public interface IMigrationRepository
    {
        MigrationLog Delete(string product, SemanticVersion version, Error err = null);

        List<MigrationLog> Get(string product = null);

        MigrationLog GetLatestVersion(string product);

        MigrationLog Save(string product, SemanticVersion version, Error err = null);
    }
}