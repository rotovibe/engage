using Funq;
using Phytel.Services.Versions;
using ServiceStack.Configuration;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Phytel.Services.Migrations
{
    public class MigrationHost : IMigrationHost
    {
        protected readonly Funq.Container _container;
        protected readonly IMigrationRepository _migrationRepository;
        protected readonly Assembly[] _assembliesWithMigrations;

        public MigrationHost(Funq.Container container, IMigrationRepository migrationRepository, params Assembly[] assembliesWithMigrations)
        {
            _container = container;
            _migrationRepository = migrationRepository;
            _assembliesWithMigrations = assembliesWithMigrations;
        }

        public void MigrateToVersion(string product, int major, int minor, int patch)
        {
            SemanticVersion version = new SemanticVersion(major, minor, patch);
            MigrateToVersion(product, version);
        }

        public void MigrateToVersion(string product, SemanticVersion versionTarget)
        {
            SemanticVersion versionCurrent = new SemanticVersion(0, 0, 0);
            MigrationLog latestMigration = GetLatestMigration(product);
            if (latestMigration != null)
            {
                versionCurrent = latestMigration.MigrationVersion;
            }

            List<IMigration> migrations = new List<IMigration>();

            if (versionCurrent < versionTarget)
            {
                //Up!
                //Grab the migrations that are part of the build
                migrations = GetCompiledMigrations(product, _assembliesWithMigrations);
                //Grab versions greater than current and less than or equal to the target. Sort acsending so applied lowest to highest.
                migrations = migrations.Where(m => m.Version > versionCurrent && (m.Version < versionTarget || m.Version.CompareTo(versionTarget) == 0)).OrderBy(m => m.Version).ToList();

                foreach (IMigration migration in migrations)
                {
                    try
                    {
                        migration.Up();
                        _migrationRepository.Save(migration.Product, migration.Version);
                    }
                    catch(Exception ex)
                    {
                        //if fail, rollback the migration
                        migration.Down();
                        //then raise the exception
                        throw ex;
                    }                    
                }
            }
            else if(versionCurrent > versionTarget)
            {
                //Down!
                //Grab the migrations that are part of the build
                migrations = GetCompiledMigrations(product, _assembliesWithMigrations);
                //Grab versions less than or equal to current and greater  the target. Sort descending so applied highest to lowest.
                migrations = migrations
                    .Where(m => 
                        (
                            m.Version < versionCurrent ||
                            m.Version.CompareTo(versionCurrent) == 0
                        )&& 
                        m.Version > versionTarget
                    )
                    .OrderByDescending(m => m.Version)
                    .ToList();

                foreach (IMigration migration in migrations)
                {
                    try
                    {
                        migration.Down();
                        _migrationRepository.Delete(migration.Product, migration.Version);
                    }
                    catch(Exception ex)
                    {
                        //if fail, put the migration back
                        migration.Up();
                        //then raise the exception
                        throw ex;
                    }                    
                }
            }
        }

        public MigrationLog GetLatestMigration(string product)
        {
            MigrationLog rvalue = null;

            var migrationLogs = _migrationRepository.Get(product);
            if (migrationLogs.Any())
            {
                rvalue = migrationLogs.OrderByDescending(m => m.MigrationVersion).FirstOrDefault();
            }

            return rvalue;
        }

        public List<IMigration> GetCompiledMigrations(string product, Assembly[] assembliesWithMigrations)
        {
            List<IMigration> rvalues = new List<IMigration>();
            List<Type> types = new List<Type>();

            foreach(Assembly assembly in assembliesWithMigrations)
            {
                types.AddRange(assembly.GetTypes());
            }
            
            foreach (Type migrationType in types.Where(t => typeof(IMigration).IsAssignableFrom(t)))
            {
                IMigration rvalue = System.Activator.CreateInstance(migrationType, _container) as IMigration;
                if (rvalue != null)
                {
                    if(rvalue.Product.ToLower() == product.ToLower())
                    {
                        rvalues.Add(rvalue);
                    }                    
                }
            }

            return rvalues;
        }
    }
}