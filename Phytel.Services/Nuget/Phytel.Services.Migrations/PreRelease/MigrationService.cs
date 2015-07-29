using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.Migrations
{
    public class MigrationService : ServiceStack.ServiceInterface.Service
    {
        protected readonly IMigrationRepository _migrationRepository;
        protected readonly List<IMigrationHandler> _migrationHandlers;

        public MigrationService(IMigrationRepository migrationRepository, List<IMigrationHandler> migrationHandlers)
        {
            _migrationRepository = migrationRepository;
            _migrationHandlers = migrationHandlers;
        }

        public MigrationsResponse Post(PostMigration request)
        {
            MigrationsResponse rvalue = new MigrationsResponse();

            foreach(IMigrationHandler migrationHandler in _migrationHandlers)
            {
                migrationHandler.Migrate(request.Product, request.Major, request.Minor, request.Patch, request.Connections);

                List<MigrationLog> migrationLogs = migrationHandler.GetLatestVersion(request.Product, request.Connections);

                rvalue.Migrations = rvalue.Migrations ?? new List<MigrationLog>();

                rvalue.Migrations.AddRange(migrationLogs);
            }

            return rvalue;
        }

        public MigrationsResponse Get(GetMigrations request)
        {
            MigrationsResponse rvalue = new MigrationsResponse();

            foreach (IMigrationHandler migrationHandler in _migrationHandlers)
            {
                List<MigrationLog> migrationLogs = migrationHandler.GetLatestVersion(request.Product, request.Connections);

                rvalue.Migrations = rvalue.Migrations ?? new List<MigrationLog>();

                rvalue.Migrations.AddRange(migrationLogs);
            }

            return rvalue;
        }
    }
}
