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
        protected readonly IMigrationHost _migrationHost;
        protected readonly IMigrationRepository _migrationRepository;
        
        public MigrationService(IMigrationHost migrationHost, IMigrationRepository migrationRepository)
        {
            _migrationHost = migrationHost;
            _migrationRepository = migrationRepository;
        }

        public MigrationResponse Post(PostMigration request)
        {
            MigrationResponse rvalue = new MigrationResponse();

            if(_migrationHost != null)
            {
                _migrationHost.MigrateToVersion(request.Product, request.Major, request.Minor, request.Patch);
            }

            rvalue.LatestMigration = _migrationHost.GetLatestMigration(request.Product);

            return rvalue;
        }

        public MigrationsResponse Get(GetMigrations request)
        {
            MigrationsResponse rvalue = new MigrationsResponse();

            rvalue.Migrations = _migrationRepository.Get(request.Product);
            
            if(rvalue.Migrations != null && rvalue.Migrations.Any())
            {
                rvalue.Migrations = rvalue.Migrations.OrderByDescending(m => m.MigrationVersion).ToList();
            }

            return rvalue;
        }
    }
}
