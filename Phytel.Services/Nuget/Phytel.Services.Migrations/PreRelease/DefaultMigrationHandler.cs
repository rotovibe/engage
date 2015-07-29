using AutoMapper;
using Phytel.Services.Versions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.Migrations
{
    public class DefaultMigrationHandler : Phytel.Services.Migrations.IMigrationHandler
    {
        protected readonly Funq.Container _container;
        protected readonly Assembly[] _assembliesWithMigrations;
        protected readonly IMappingEngine _mappingEngine;
        protected readonly IMigrationRepository _migrationRepository;

        public DefaultMigrationHandler(Funq.Container container, IMigrationRepository migrationRepository, IMappingEngine mappingEngine, params Assembly[] assembliesWithMigrations)
        {
            _container = container;
            _assembliesWithMigrations = assembliesWithMigrations;
            _mappingEngine = mappingEngine;
            _migrationRepository = migrationRepository;
        }

        public void Migrate(string product, int major, int minor, int patch, List<Connection> connectionOverrides)
        {
            SemanticVersion version = new SemanticVersion(major, minor, patch);
            Migrate(product, version, connectionOverrides);
        }

        public void Migrate(string product, SemanticVersion versionTarget, List<Connection> connectionOverrides)
        {
            List<IMigration> migrations = OnMigrateGetMigrations(product, _assembliesWithMigrations);

            OnMigrate(product, versionTarget, migrations);
        }

        public List<MigrationLog> GetLatestVersion(string product, List<Connection> connectionOverrides)
        {
            List<MigrationLog> rvalues = new List<MigrationLog>();

            MigrationLog rvalue = _migrationRepository.GetLatestVersion(product);
            rvalues.Add(rvalue);

            return rvalues;
        }

        protected virtual void OnMigrate(string product, SemanticVersion versionTarget, List<IMigration> migrations)
        {
            SemanticVersion versionCurrent = new SemanticVersion(0, 0, 0);
            MigrationLog latestMigration = _migrationRepository.GetLatestVersion(product);
            if (latestMigration != null)
            {
                versionCurrent = latestMigration.MigrationVersion;
            }

            List<IMigration> migrationToApply = new List<IMigration>();

            if (versionCurrent < versionTarget)
            {
                migrationToApply = migrations
                    .Where(m => 
                        m.Version > versionCurrent && 
                        (
                            m.Version < versionTarget || m.Version.CompareTo(versionTarget) == 0
                        )
                    )
                    .OrderBy(m => m.Version)
                    .ToList();

                foreach (IMigration migration in migrationToApply)
                {
                    OnMigrateUp(migration);
                }
            }
            else if (versionCurrent > versionTarget)
            {
                migrationToApply = migrations
                    .Where(m =>
                        (
                            m.Version < versionCurrent ||
                            m.Version.CompareTo(versionCurrent) == 0
                        ) &&
                        m.Version > versionTarget
                    )
                    .OrderByDescending(m => m.Version)
                    .ToList();

                foreach (IMigration migration in migrationToApply)
                {
                    OnMigrateDown(migration);
                }
            }            
        }

        protected virtual void OnMigrateUp(IMigration migration)
        {
            try
            {
                migration.Up();
                _migrationRepository.Save(migration.Product, migration.Version);
            }
            catch (Exception ex)
            {
                Error error = _mappingEngine.Map<Error>(ex);
                _migrationRepository.Delete(migration.Product, migration.Version, error);
                throw ex;
            }                
        }

        protected virtual void OnMigrateDown(IMigration migration)
        {
            try
            {
                migration.Down();
                _migrationRepository.Delete(migration.Product, migration.Version);
            }
            catch (Exception ex)
            {
                Error error = _mappingEngine.Map<Error>(ex);
                _migrationRepository.Save(migration.Product, migration.Version, error);
                throw ex;
            }                 
        }

        protected virtual List<IMigration> OnMigrateGetMigrations(string product, Assembly[] assembliesWithMigrations)
        {
            List<IMigration> rvalues = new List<IMigration>();
            List<Type> types = new List<Type>();

            foreach (Assembly assembly in assembliesWithMigrations)
            {
                types.AddRange(assembly.GetTypes());
            }

            foreach (Type migrationType in types.Where(t => typeof(IMigration).IsAssignableFrom(t)))
            {
                IMigration rvalue = System.Activator.CreateInstance(migrationType, _container) as IMigration;
                if (rvalue != null)
                {
                    if (rvalue.Product.ToLower() == product.ToLower())
                    {
                        rvalues.Add(rvalue);
                    }
                }
            }

            return rvalues;
        }
    }
}
