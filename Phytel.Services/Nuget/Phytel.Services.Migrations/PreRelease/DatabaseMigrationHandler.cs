using AutoMapper;
using Phytel.Services.Dates;
using Phytel.Services.Mongo;
using Phytel.Services.Versions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.Migrations
{
    public class DatabaseMigrationHandler : IMigrationHandler
    {
        protected readonly Funq.Container _container;
        protected readonly Assembly[] _assembliesWithMigrations;
        protected readonly IMigrationDatabaseLogRepository _migrationDatabaseLogRepository;
        protected readonly IDatabaseFactory _databaseFactory;
        protected readonly IMappingEngine _mappingEngine;
        protected readonly Func<Funq.Container, List<Connection>> _connectionsFactory;

        public DatabaseMigrationHandler(
            Funq.Container container, 
            IMigrationDatabaseLogRepository migrationDatabaseLogRepository, 
            IDateTimeProxy dateTimeProxy, 
            IDatabaseFactory databaseFactory, 
            IMappingEngine mappingEngine,
            Func<Funq.Container, List<Connection>> connectionsFactory,
            params Assembly[] assembliesWithMigrations
            )
        {
            _container = container;
            _migrationDatabaseLogRepository = migrationDatabaseLogRepository;
            _assembliesWithMigrations = assembliesWithMigrations;
            _databaseFactory = databaseFactory;
            _mappingEngine = mappingEngine;
            _connectionsFactory = connectionsFactory;
        }

        public void Migrate(string product, int major, int minor, int patch, List<Connection> connectionOverrides)
        {
            SemanticVersion version = new SemanticVersion(major, minor, patch);
            Migrate(product, version, connectionOverrides);
        }

        public void Migrate(string product, SemanticVersion versionTarget, List<Connection> connectionOverrides)
        {
            List<IDatabaseMigration> migrations = OnMigrateGetDatabaseMigrations(product, _assembliesWithMigrations);

            List<Connection> connections = connectionOverrides;

            if(connections == null || connections.Count == 0)
            {
                connections = _connectionsFactory(_container);
            }

            OnMigrate(product, versionTarget, migrations, connections);
        }

        protected virtual void OnMigrate(string product, SemanticVersion versionTarget, List<IDatabaseMigration> migrations, List<Connection> connections)
        {
            foreach (Connection connection in connections)
            {
                OnMigrateConnection(product, versionTarget, migrations, connection);
            }            
        }

        protected virtual void OnMigrateConnection(string product, SemanticVersion versionTarget, List<IDatabaseMigration> migrations, Connection connection)
        {
            Database db = _databaseFactory.Create(connection);
            SemanticVersion versionCurrent = new SemanticVersion(0, 0, 0);
            MigrationLog latestMigration = _migrationDatabaseLogRepository.GetLatestVersion(product, db);
            if (latestMigration != null)
            {
                versionCurrent = latestMigration.MigrationVersion;
            }

            List<IDatabaseMigration> migrationsToApply = new List<IDatabaseMigration>();

            if(versionCurrent < versionTarget)
            {
                migrationsToApply = migrations
                    .Where(m =>
                        m.Version > versionCurrent &&
                        (
                            m.Version < versionTarget || m.Version.CompareTo(versionTarget) == 0
                        )
                    )
                    .OrderBy(m => m.Version)
                    .ToList();

                foreach (IDatabaseMigration migration in migrationsToApply)
                {
                    OnMigrateUp(migration, db, connection);
                }
            }
            else if (versionCurrent > versionTarget)
            {
                migrationsToApply = migrations
                    .Where(m =>
                        (
                            m.Version < versionCurrent ||
                            m.Version.CompareTo(versionCurrent) == 0
                        ) &&
                        m.Version > versionTarget
                    )
                    .OrderByDescending(m => m.Version)
                    .ToList();

                foreach (IDatabaseMigration migration in migrationsToApply)
                {
                    OnMigrateDown(migration, db, connection);
                }
            }
        }

        protected virtual void OnMigrateUp(IDatabaseMigration migration, Database db, Connection connection)
        {
            try
            {
                migration.Up(connection);
                _migrationDatabaseLogRepository.Save(migration.Product, migration.Version, db);
                _migrationDatabaseLogRepository.DeleteVersionsLessThan(migration.Product, migration.Version, db);
            }
            catch(Exception ex)
            {
                Error error = _mappingEngine.Map<Error>(ex);
                _migrationDatabaseLogRepository.Delete(migration.Product, migration.Version, db, error);
                throw ex;
            }
        }

        protected virtual void OnMigrateDown(IDatabaseMigration migration, Database db, Connection connection)
        {
            try
            {
                migration.Down(connection);
                _migrationDatabaseLogRepository.Delete(migration.Product, migration.Version, db);
            }
            catch (Exception ex)
            {
                Error error = _mappingEngine.Map<Error>(ex);
                _migrationDatabaseLogRepository.Save(migration.Product, migration.Version, db, error);
                throw ex;
            }
        }

        protected virtual List<IDatabaseMigration> OnMigrateGetDatabaseMigrations(string product, Assembly[] assembliesWithMigrations)
        {
            List<IDatabaseMigration> rvalues = new List<IDatabaseMigration>();
            List<Type> types = new List<Type>();

            foreach (Assembly assembly in assembliesWithMigrations)
            {
                types.AddRange(assembly.GetTypes());
            }

            foreach (Type migrationType in types.Where(t => typeof(IDatabaseMigration).IsAssignableFrom(t)))
            {
                IDatabaseMigration rvalue = System.Activator.CreateInstance(migrationType, _container) as IDatabaseMigration;
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

        public List<MigrationLog> GetLatestVersion(string product, List<Connection> connectionOverrides)
        {
            List<Database> dbs = new List<Database>();
            if (connectionOverrides != null && connectionOverrides.Any())
            {
                foreach (Connection connectionOverride in connectionOverrides)
                {
                    Database db = _databaseFactory.Create(connectionOverride);
                    if (db != null)
                    {
                        dbs.Add(db);
                    }
                }
            }

            return _migrationDatabaseLogRepository.GetLatestVersions(product, dbs);
        }
    }
}
