using AutoMapper;
using MongoDB.Bson;
using Phytel.Services.AppSettings;
using Phytel.Services.Dates;
using Phytel.Services.IOC;
using Phytel.Services.Migrations.Mongo;
using Phytel.Services.Mongo.Repository;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.Migrations
{
    public class MigrationContainer : ContainerDecorator
    {
        public const string ScopeName = "Migrations";

        protected readonly Assembly[] _assembliesWithMigrations;
        protected readonly Func<Funq.Container, List<Connection>> _connectionsFactory;

        public MigrationContainer(ContainerBuilder containerBuilder, Func<Funq.Container, List<Connection>> connectionsFactory = null, params Assembly[] assembliesWithMigrations)
            :base(containerBuilder)
        {
            _assembliesWithMigrations = assembliesWithMigrations;
            _connectionsFactory = connectionsFactory;
        }

        protected override void OnBuild(Funq.Container container)
        {
            Mapper.CreateMap<MigrationLogEntity, MigrationLog>().ReverseMap();
            Mapper.CreateMap<MigrationDatabaseLogEntity, MigrationLog>().ReverseMap();
            Mapper.CreateMap<Exception, Error>();

            if(container.TryResolve<IMappingEngine>() == null)
            {
                container.Register<IMappingEngine>(Mapper.Engine);
            }
            if(container.TryResolve<IDateTimeProxy>() == null)
            {
                container.Register<IDateTimeProxy>(new DateTimeLocalProxy());
            }
            if(container.TryResolve<IAppSettingsProvider>() == null)
            {
                container.Register<IAppSettingsProvider>(
                    new EagerLoadedAppSettingsProvider(new Dictionary<string, string>()));
            }

            container.Register<IMigrationHandler>(typeof(DefaultMigrationHandler).FullName, c =>
                new DefaultMigrationHandler(
                    c,
                    c.Resolve<IMigrationRepository>(),
                    c.Resolve<IMappingEngine>(),
                    _assembliesWithMigrations
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            if(_connectionsFactory != null)
            {
                container.Register<IMigrationHandler>(typeof(DatabaseMigrationHandler).FullName, c =>
                new DatabaseMigrationHandler(
                    c,
                    c.Resolve<IMigrationDatabaseLogRepository>(),
                    c.Resolve<IDateTimeProxy>(),
                    c.Resolve<IDatabaseFactory>(),
                    c.Resolve<IMappingEngine>(),
                    _connectionsFactory,
                    _assembliesWithMigrations
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);
            }           

            container.Register<MigrationService>(c =>
            {
                List<Type> types = new List<Type>();
                Assembly assembly = Assembly.GetExecutingAssembly();
                types.AddRange(assembly.GetTypes());
                Assembly callingAssembly = Assembly.GetCallingAssembly();
                types.AddRange(callingAssembly.GetTypes());

                List<IMigrationHandler> migrationHandlers = new List<IMigrationHandler>();

                foreach (Type migrationType in types.Where(t => typeof(IMigrationHandler).IsAssignableFrom(t)))
                {
                    IMigrationHandler migrationHandler = c.TryResolveNamed<IMigrationHandler>(migrationType.FullName);
                    if(migrationHandler != null)
                    {
                        migrationHandlers.Add(migrationHandler);
                    }
                }

                return new MigrationService(
                    c.Resolve<IMigrationRepository>(), 
                    migrationHandlers
                );

            }).ReusedWithin(Funq.ReuseScope.Request);

            container.Register<IMigrationDatabaseLogRepository>(c =>
                new MigrationDatabaseLogRepository(
                    c.ResolveNamed<IRepositoryMongo>(ScopeName),
                    c.Resolve<IMappingEngine>(),
                    c.Resolve<IDateTimeProxy>()
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            container.Register<IDatabaseFactory>(c =>
                new DatabaseFactory());

            container.Register<IRepositoryMongo>(ScopeName, c =>
                new RepositoryMongo<MigrationMongoContext>(
                    new UnitOfWorkMongo<MigrationMongoContext>(
                        c.Resolve<IAppSettingsProvider>().Get(Constants.AppSettingKeys.DatabaseConnectionName, Constants.AppSettingDefaultValues.DatabaseConnectionName),
                        false
                        )
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            container.Register<IMigrationRepository>(c =>
                new MigrationRepository(
                    c.ResolveNamed<IRepositoryMongo>(ScopeName),
                    c.Resolve<IMappingEngine>(),
                    c.Resolve<IDateTimeProxy>()
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);
        }
    }
}
